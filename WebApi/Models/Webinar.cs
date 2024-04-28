namespace WebApi.Models;

public class Webinar
{
    public int Id { get; set; }
    public required string Title { get; set; }

    public List<Answer>? Answers { get; set; }
}