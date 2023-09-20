using Microsoft.EntityFrameworkCore;
using OnEntitySharedLogic.DatabaseGenericRepository;
using StudentExamination.Api.Core.Models.ExaminationModels;

namespace StudentExamination.Api.Infrastructure.Repository;

public class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Exam>? Exams { get; set; }
    public DbSet<Problem>? Problems { get; set; }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exam>()
            .HasKey(exam => exam.Id);

        modelBuilder.Entity<Problem>()
            .HasKey(problem => problem.Id);

        modelBuilder.Entity<AnswerOption>()
            .HasKey(option => option.Id);

        modelBuilder.Entity<CorrectAnswer>()
            .HasKey(answer => answer.Id);

        modelBuilder.Entity<ExamAttendance>()
            .HasKey(attendance => attendance.Id);
        
        //Make required
        modelBuilder.Entity<Exam>()
            .Property(exam => exam.ExamDuration)
            .IsRequired();
        
        modelBuilder.Entity<Exam>()
            .Property(exam => exam.CourseId)
            .IsRequired();
        
        modelBuilder.Entity<Exam>()
            .Property(exam => exam.AvailableFrom)
            .IsRequired();
        
        modelBuilder.Entity<Exam>()
            .Property(exam => exam.AvailableUntil)
            .IsRequired();

        modelBuilder.Entity<Exam>()
            .Property(exam => exam.PartialGradingAllowed)
            .IsRequired();

        modelBuilder.Entity<ExamAttendance>()
            .Property(exam => exam.StudentId)
            .IsRequired();
        
        modelBuilder.Entity<Problem>()
            .Property(problem => problem.Text)
            .IsRequired();
        
        modelBuilder.Entity<Problem>()
            .Property(problem => problem.ProblemType)
            .IsRequired();
        
        modelBuilder.Entity<Problem>()
            .Property(problem => problem.Points)
            .IsRequired();

        modelBuilder.Entity<Problem>()
            .Property(problem => problem.ExamId)
            .IsRequired();

        modelBuilder.Entity<AnswerOption>()
            .Property(option => option.Answer)
            .IsRequired();
        
        modelBuilder.Entity<CorrectAnswer>()
            .Property(answer => answer.Answer)
            .IsRequired();
        
        //Create relations
        modelBuilder.Entity<Exam>()
            .HasMany(exam => exam.Problems)
            .WithOne(problem => problem.Exam)
            .HasForeignKey(problem => problem.ExamId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Problem>()
            .HasMany(problem => problem.AnswerOptions)
            .WithOne(option => option.Problem)
            .HasForeignKey(option => option.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Problem>()
            .HasMany(problem => problem.CorrectAnswers)
            .WithOne(answer => answer.Problem)
            .HasForeignKey(answer => answer.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Exam>()
            .HasMany(exam => exam.ExamAttendances)
            .WithOne(attendance => attendance.Exam)
            .HasForeignKey(attendance => attendance.ExamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}