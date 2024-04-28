using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using WebApi;
using WebApi.Api;
using WebApi.MassTransit.Consumers;
using WebApi.Services;
using WebApi.Services.Initialize;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<ApplicationDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb"));
    o.UseSnakeCaseNamingConvention();
});
builder.Services.AddHttpClient<PerformNluAnswerConsumer>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetConnectionString("NluService")!);
});
builder.Services.AddCors();
builder.Services.AddHostedService<MigrateDb<ApplicationDbContext>>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddMassTransit(o =>
{
    o.UsingRabbitMq(((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
        cfg.ConfigureEndpoints(context, new SnakeCaseEndpointNameFormatter(true));
    }));
    o.AddConsumer<PerformNluAnswerConsumer>(c =>
    {
        c.UseMessageRetry(
            r => r.Intervals(
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            )
        );
        c.ConcurrentMessageLimit = 3;
    });
});

var app = builder.Build();
app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

var api = app.MapGroup("api");
api
    .MapGroup("chat")
    .MapChat()
    .WithTags("chat");

api
    .MapGroup("analytics")
    .MapAnalytics()
    .WithTags("analytics");


app.UseSwagger();
app.UseSwaggerUI();

app.Run();