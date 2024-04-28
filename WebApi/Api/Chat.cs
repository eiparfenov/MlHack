using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebApi.Api;

public static class Chat
{
    public static RouteGroupBuilder MapChat(this RouteGroupBuilder chat)
    {
        chat
            .MapPost("post_chat_result", async Task<IResult> (
                [FromBody] PostChatRequest request,
                [FromServices] IChatService chatService
                ) =>
            {
                await chatService.PostChat(
                    request.Name,
                    request.Teacher,
                    request.Webinar,
                    request.Course,
                    request.Question1,
                    request.Question2 == null
                        ? null
                        :
                        [
                            request.Question2.Question1,
                            request.Question2.Question2,
                            request.Question2.Question3,
                            request.Question2.Question4,
                        ]
                );
                return Results.Ok();
            })
            .WithOpenApi()
            .WithSummary("Ручка для обработки результатов чата");
        return chat;
    }

    /// <summary>
    /// Запрос на обработку результатов чата
    /// </summary>
    class PostChatRequest
    {
        /// <summary>
        /// ФИО преподавателя
        /// </summary>
        public required string Teacher { get; set; }
        
        /// <summary>
        /// Название вебинанара
        /// </summary>
        public required string Webinar { get; set; }
        
        /// <summary>
        /// Название образовательной программы
        /// </summary>
        public required string Course { get; set; }
        
        /// <summary>
        /// ФИО обучающегося
        /// </summary>
        public required string Name { get; set; }
        
        /// <summary>
        /// Желаение дать обратную связь
        /// </summary>
        [JsonPropertyName("question_1")] public required int Question1 { get; set; }
        
        /// <summary>
        /// Ответы на открытае вопросы
        /// </summary>
        [JsonPropertyName("question_2")] public QuestionBlock? Question2 { get; set; }
    }

    class QuestionBlock
    {
        /// <summary>
        /// Ответ на вопрос 1
        /// </summary>
        [JsonPropertyName("question_1")] public required string Question1 { get; set; }
        
        
        /// <summary>
        /// Ответ на вопрос 2
        /// </summary>
        [JsonPropertyName("question_2")] public required string Question2 { get; set; }
        
        
        /// <summary>
        /// Ответ на вопрос 3
        /// </summary>
        [JsonPropertyName("question_3")] public required string Question3 { get; set; }
        
        
        /// <summary>
        /// Ответ на вопрос 4
        /// </summary>
        [JsonPropertyName("question_4")] public required string Question4 { get; set; }
    }
}