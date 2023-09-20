using Internship.UniversityScheduler.Api.Core.Models;
using Internship.UniversityScheduler.Api.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using OnEntitySharedLogic.DatabaseGenericRepository;

namespace Internship.UniversityScheduler.Api.Infrastructure;

//here i will define the model mappings to the project's database
public class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Student>? Students { get; set; }
    public DbSet<Professor>? Professors { get; set; }
    public DbSet<Catalogue>? Catalogues { get; set; }
    public DbSet<Grade>? Grades { get; set; }
    public DbSet<UniversityGroup>? UniversityGroups { get; set; }
    public DbSet<Attendance>? Attendances { get; set; }
    public DbSet<Course>? Courses { get; set; }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Add primary keys
        modelBuilder.Entity<Student>()
            .HasKey(student => student.Id);

        modelBuilder.Entity<Professor>()
            .HasKey(professor => professor.Id);

        modelBuilder.Entity<Catalogue>()
            .HasKey(catalogue => catalogue.Id);

        modelBuilder.Entity<Grade>()
            .HasKey(grade => grade.Id);

        modelBuilder.Entity<Course>()
            .HasKey(course => course.Id);

        modelBuilder.Entity<UniversityGroup>()
            .HasKey(group => group.Id);

        modelBuilder.Entity<Attendance>()
            .HasKey(attendance => attendance.Id);

        modelBuilder.Entity<Student>()
            .HasIndex(student => student.PhoneNumber)
            .IsUnique();
        
        modelBuilder.Entity<Student>()
            .HasIndex(student => student.Email)
            .IsUnique();

        modelBuilder.Entity<Professor>()
            .HasIndex(professor => professor.PhoneNumber)
            .IsUnique();
        
        modelBuilder.Entity<Professor>()
            .HasIndex(student => student.Email)
            .IsUnique();
        
        modelBuilder.Entity<Student>()
            .HasIndex(student => student.PersonalEmail)
            .IsUnique();

        //Set required properties for models
        modelBuilder.Entity<Student>()
            .Property(student => student.PersonalEmail)
            .IsRequired();
        
        modelBuilder.Entity<Student>()
            .Property(student => student.Email)
            .IsRequired();

        modelBuilder.Entity<Student>()
            .Property(student => student.FirstName)
            .IsRequired();

        modelBuilder.Entity<Student>()
            .Property(student => student.LastName)
            .IsRequired();

        modelBuilder.Entity<Student>()
            .Property(student => student.FullName)
            .IsRequired();

        modelBuilder.Entity<Student>()
            .Property(student => student.StudyYear)
            .IsRequired();
        
        modelBuilder.Entity<Student>()
            .Property(student => student.BirthdayDate)
            .IsRequired();
        
        modelBuilder.Entity<Student>()
            .Property(student => student.PhoneNumber)
            .IsRequired();

        modelBuilder.Entity<Professor>()
            .Property(professor => professor.Email)
            .IsRequired();

        modelBuilder.Entity<Professor>()
            .Property(professor => professor.FirstName)
            .IsRequired();

        modelBuilder.Entity<Professor>()
            .Property(professor => professor.LastName)
            .IsRequired();

        modelBuilder.Entity<Professor>()
            .Property(professor => professor.Speciality)
            .IsRequired();
        
        modelBuilder.Entity<Professor>()
            .Property(professor => professor.BirthdayDate)
            .IsRequired();
        
        modelBuilder.Entity<Professor>()
            .Property(professor => professor.PhoneNumber)
            .IsRequired();

        modelBuilder.Entity<Grade>()
            .Property(grade => grade.Value)
            .IsRequired();

        modelBuilder.Entity<Grade>()
            .Property(grade => grade.DateOfGrade)
            .IsRequired();

        modelBuilder.Entity<Course>()
            .Property(course => course.NumberOfCredits)
            .IsRequired();

        modelBuilder.Entity<Course>()
            .Property(course => course.Domain)
            .IsRequired();

        modelBuilder.Entity<Course>()
            .Property(course => course.Type)
            .IsRequired();

        modelBuilder.Entity<Course>()
            .Property(course => course.CourseName)
            .IsRequired();

        modelBuilder.Entity<Attendance>()
            .Property(attendance => attendance.DateOfTheCourse)
            .IsRequired();

        modelBuilder.Entity<Attendance>()
            .Property(attendance => attendance.TimeOfTheCourse)
            .IsRequired();

        modelBuilder.Entity<UniversityGroup>()
            .Property(group => group.Name)
            .IsRequired();

        modelBuilder.Entity<UniversityGroup>()
            .Property(group => group.Specialization)
            .IsRequired();

        modelBuilder.Entity<UniversityGroup>()
            .Property(group => group.MaxSize)
            .IsRequired();

        modelBuilder.Entity<UniversityGroup>()
            .Property(group => group.NumberOfMembers)
            .IsRequired();

        //Fix DateOnly and TimeOnly mapping problems
        modelBuilder.Entity<Student>()
            .Property(student => student.BirthdayDate)
            .HasConversion(new DateOnlyToStringConverter());

        modelBuilder.Entity<Professor>()
            .Property(professor => professor.BirthdayDate)
            .HasConversion(new DateOnlyToStringConverter());

        modelBuilder.Entity<Attendance>()
            .Property(attendance => attendance.DateOfTheCourse)
            .HasConversion(new DateOnlyToStringConverter());

        modelBuilder.Entity<Attendance>()
            .Property(attendance => attendance.TimeOfTheCourse)
            .HasConversion(new TimeOnlyToStringConverter());

        //Create relations and add foreign keys
        modelBuilder.Entity<Catalogue>()
            .HasMany(catalogue => catalogue.Grades)
            .WithOne(grade => grade.Catalogue)
            .HasForeignKey(grade => grade.CatalogueId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Professor>()
            .HasMany(professor => professor.Courses)
            .WithOne(course => course.Professor)
            .HasForeignKey(course => course.ProfessorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Course>()
            .HasMany(course => course.Attendances)
            .WithOne(attendance => attendance.Course)
            .HasForeignKey(attendance => attendance.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Course>()
            .HasMany(course => course.Grades)
            .WithOne(grade => grade.Course)
            .HasForeignKey(grade => grade.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Student>()
            .HasMany(student => student.Grades)
            .WithOne(grade => grade.Student)
            .HasForeignKey(grade => grade.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Student>()
            .HasMany(student => student.Attendances)
            .WithOne(attendance => attendance.Student)
            .HasForeignKey(attendance => attendance.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UniversityGroup>()
            .HasMany(group => group.Students)
            .WithOne(student => student.UniversityGroup)
            .HasForeignKey(student => student.UniversityGroupId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<UniversityGroup>()
            .HasOne(group => group.Catalogue)
            .WithOne(catalogue => catalogue.UniversityGroup)
            .HasForeignKey<Catalogue>(catalogue => catalogue.UniversityGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}