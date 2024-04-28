using System.Text.Json.Serialization;
using MassTransit;
using WebApi.MassTransit.Contracts;
using WebApi.Models;

namespace WebApi.MassTransit.Consumers;

public class PerformNluAnswerConsumer(
    ApplicationDbContext db,
    HttpClient httpClient,
    ILogger<PerformNluAnswerConsumer> logger
    ): IConsumer<PerformNluAnswer>
{
    public async Task Consume(ConsumeContext<PerformNluAnswer> context)
    {
        var msg = context.Message;
        
        var httpResponse = await httpClient.PostAsJsonAsync("", new NluRequest() { Answers = msg.Answers });
        httpResponse.EnsureSuccessStatusCode();
        
        var response = await httpResponse.Content.ReadFromJsonAsync<NluResponse>();
        logger.LogInformation("{AnswerId}", msg.AnswerId);
        var modelResponse = new ModelResponse()
        {
            AnswerId = msg.AnswerId,
            Object = response!.Object,
            IsPositive = response.IsPositive,
            IsRelevant = response.IsRelevant
        };
        await db.ModelResponses.AddAsync(modelResponse);
        await db.SaveChangesAsync();
    }

    class NluRequest
    {
        public ICollection<string> Answers { get; set; } = [];
    }

    class NluResponse
    {
        [JsonPropertyName("is_positive")] public bool IsPositive { get; set; }
        [JsonPropertyName("object")] public int Object { get; set; }
        [JsonPropertyName("is_relevant")] public bool IsRelevant { get; set; }
    }
}