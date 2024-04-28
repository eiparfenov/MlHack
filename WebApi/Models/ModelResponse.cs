namespace WebApi.Models;

public class ModelResponse
{
    public int Id { get; set; }

    public bool IsPositive { get; set; }
    public int Object { get; set; }
    public bool IsRelevant { get; set; }
    
    public int AnswerId { get; set; }
    public Answer? Answer { get; set; }
}