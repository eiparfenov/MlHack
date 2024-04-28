using MassTransit;
using Microsoft.EntityFrameworkCore;
using WebApi.MassTransit.Contracts;
using WebApi.Models;

namespace WebApi.Services;

public interface IChatService
{
    Task PostChat(string studentName, string teacherName, string webinarTitle, string courseTitle, int ready, string[]? answers);
}

public class ChatService(
    ApplicationDbContext db,
    IBus bus
): IChatService
{
    public async Task PostChat(string studentName, string teacherName, string webinarTitle, string courseTitle, int ready, string[]? answers)
    {
        var teacher = await db.Teachers.SingleOrDefaultAsync(t => t.Name == teacherName);
        if (teacher == null)
        {
            teacher = new Teacher() { Name = teacherName };
            await db.Teachers.AddAsync(teacher);
        }

        var webinar = await db.Webinars.SingleOrDefaultAsync(w => w.Title == webinarTitle);
        if (webinar == null)
        {
            webinar = new Webinar() { Title = webinarTitle };
            await db.Webinars.AddAsync(webinar);
        }

        var course = await db.Courses.SingleOrDefaultAsync(c => c.Title == courseTitle);
        if (course == null) 
        {
            course = new Course() { Title = courseTitle };
            await db.AddAsync(course);
        }

        var student = await db.Students.SingleOrDefaultAsync(s => s.Name == studentName);
        if (student == null)
        {
            student = new Student() { Name = studentName };
            await db.Students.AddAsync(student);
        }

        var answer = new Answer()
        {
            ReadyToAnswer = ready,
            Answers = answers,
            Teacher = teacher,
            Student = student,
            Course = course,
            Webinar = webinar,
        };
        await db.AddAsync(answer);
        await db.SaveChangesAsync();
        
        
        if(answers != null && answers.Length != 0)
        {
            await bus.Publish(new PerformNluAnswer()
            {
                AnswerId = answer.Id,
                Answers = answers
            });
        }
    }
}