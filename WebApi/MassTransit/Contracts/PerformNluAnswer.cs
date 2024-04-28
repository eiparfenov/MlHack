namespace WebApi.MassTransit.Contracts;

public sealed record PerformNluAnswer
{
    public int AnswerId { get; set; }
    public ICollection<string> Answers { get; set; } = [];
}