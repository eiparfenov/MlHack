namespace WebApi.Models;

public class Course
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public List<Answer>? Answers { get; set; }

}