using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Api;

public static class Analytics
{
    public static RouteGroupBuilder MapAnalytics(this RouteGroupBuilder analytics)
    {
        analytics
            .MapPost("options", async Task<Ok<OptionsResponse>> (
                [FromServices] IAnalyticsService analyticsService) =>
            {
                var opts = await analyticsService.GetAllOptions();
                return TypedResults.Ok(new OptionsResponse()
                {
                    Teachers = opts.teachers,
                    Courses = opts.courses,
                    Webinars = opts.webinars
                });
            })
            .WithOpenApi();

        analytics
            .MapPost("model_responses", async Task<Ok<ModelResponsesResponse>> (
                [FromQuery] string? teacherName,
                [FromQuery] string? courseTitle,
                [FromQuery] string? webinarTitle,
                [FromServices] IAnalyticsService analyticsService
            ) =>
            {
                var res = await analyticsService.GetModelResponses(teacherName, courseTitle, webinarTitle);
                return TypedResults.Ok(new ModelResponsesResponse()
                {
                    ModelResponses =
                    [
                        ..res.Select(r => new ModelResponseDto()
                        {
                            Object = r.Object,
                            IsRelevant = r.IsRelevant,
                            Answers = r.Answers,
                            IsPositive = r.IsPositive
                        })
                    ]
                });
            })
            .WithOpenApi();

        return analytics;
    }

    class OptionsResponse
    {
        public ICollection<string> Teachers { get; set; } = [];
        public ICollection<string> Courses { get; set; } = [];
        public ICollection<string> Webinars { get; set; } = [];
    }

    class ModelResponseDto
    {
        public bool IsPositive { get; set; }
        public int Object { get; set; }
        public bool IsRelevant { get; set; }
        public ICollection<string> Answers { get; set; } = [];
    }

    class ModelResponsesResponse
    {
        public ICollection<ModelResponseDto> ModelResponses { get; set; } = [];
    }
}