namespace WebApi.Models;

public class Teacher
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Answer>? Answers { get; set; }

}