namespace WebApi.Models;

public class Answer
{
    public int Id { get; set; }

    public int ReadyToAnswer { get; set; }
    public ICollection<string>? Answers { get; set; }

    public int StudentId { get; set; }
    public Student? Student { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public int WebinarId { get; set; }
    public Webinar? Webinar { get; set; }

    public int TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    public ModelResponse? ModelResponse { get; set; }
}