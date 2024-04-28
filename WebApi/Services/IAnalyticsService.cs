using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Services;

public interface IAnalyticsService
{
    Task<(ICollection<string> teachers, ICollection<string> courses, ICollection<string> webinars)> GetAllOptions();

    Task<ICollection<(ICollection<string> Answers, int Object, bool IsRelevant, bool IsPositive)>> GetModelResponses(
        string? teacher, string? course, string? webinar);
}

public class AnalyticsService (ApplicationDbContext db) : IAnalyticsService
{
    public async Task<(ICollection<string> teachers, ICollection<string> courses, ICollection<string> webinars)> GetAllOptions()
    {
        var teachers = await db.Teachers.Select(t => t.Name).ToListAsync();
        var courses = await db.Courses.Select(c => c.Title).ToListAsync();
        var webinars = await db.Webinars.Select(w => w.Title).ToListAsync();
        return (teachers, courses, webinars);
    }

    public async Task<ICollection<(ICollection<string> Answers, int Object, bool IsRelevant, bool IsPositive)>> GetModelResponses(string? teacher, string? course, string? webinar)
    {
        IQueryable<ModelResponse> answers = db.ModelResponses;
        if (!string.IsNullOrEmpty(teacher))
        {
            answers = answers.Where(a => a.Answer!.Teacher!.Name == teacher);
        }

        if (!string.IsNullOrEmpty(course))
        {
            answers = answers.Where(c => c.Answer!.Course!.Title == course);
        }
        
        if (!string.IsNullOrEmpty(webinar))
        {
            answers = answers.Where(c => c.Answer!.Webinar!.Title == webinar);
        }
        var result = await answers
            .Select(m => new { m.Answer!.Answers, m.Object, m.IsRelevant, m.IsPositive })
            .ToListAsync();
        return [.. result.Select(x => (x.Answers, x.Object, x.IsRelevant, x.IsPositive))!];
    }
}