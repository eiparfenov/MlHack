using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi;

public class ApplicationDbContext: DbContext
{
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<ModelResponse> ModelResponses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Webinar> Webinars { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>()
            .ToTable("answer")
            .HasOne(a => a.ModelResponse)
            .WithOne(m => m.Answer)
            .HasForeignKey<ModelResponse>(m => m.AnswerId);
        modelBuilder.Entity<Course>()
            .ToTable("course")
            .Property(c => c.Title)
            .HasMaxLength(100);
        modelBuilder.Entity<ModelResponse>()
            .ToTable("model_response");
        modelBuilder.Entity<Student>()
            .ToTable("student")
            .Property(c => c.Name)
            .HasMaxLength(100);
        modelBuilder.Entity<Teacher>()
            .ToTable("teacher")
            .Property(c => c.Name)
            .HasMaxLength(100);
        modelBuilder.Entity<Webinar>()
            .ToTable("webinar")
            .Property(c => c.Title)
            .HasMaxLength(100);
    }
}