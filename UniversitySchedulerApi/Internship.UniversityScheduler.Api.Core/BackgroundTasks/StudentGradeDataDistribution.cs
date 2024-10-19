using System.Text;
using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
using Internship.UniversityScheduler.Library.SharedModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.RabbitMq;
using ProtoBuf.WellKnownTypes;
using Quartz;

namespace Internship.UniversityScheduler.Api.Core.BackgroundTasks;

public class StudentGradeDataDistribution : IJob
{
    private readonly ILogger<StudentGradeDataDistribution> _logger;
    private readonly IGradeService _gradeService;
    private readonly IStudentService _studentService;
    private readonly ICourseService _courseService;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IConfiguration _configuration;
    
    public StudentGradeDataDistribution(
        ILogger<StudentGradeDataDistribution> logger, 
        IGradeService gradeService,
        IStudentService studentService,
        ICourseService courseService,
        IMessagePublisher messagePublisher,
        IConfiguration configuration)
    {
        _logger = logger;
        _gradeService = gradeService;
        _studentService = studentService;
        _courseService = courseService;
        _messagePublisher = messagePublisher;
        _configuration = configuration;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var groupedGrades = (await _gradeService.GetAllGradesAsync())
            .Where(grade => grade.DateOfGrade.Date >= DateTime.UtcNow.Date.AddDays(-7))
            .GroupBy(grade => grade.StudentId);

        foreach (var groupedGrade in groupedGrades)
        {
            var existingStudent = await _studentService.GetStudentByIdAsync(groupedGrade.Key);

            var studentGradeEmail = new StudentGradesEmail
            {
                StudentEmail = existingStudent.PersonalEmail
            };
            
            foreach (var grade in groupedGrade)
            {
                
                var existingCourse = await _courseService.GetCourseByIdAsync(grade.CourseId);
                
                studentGradeEmail.Grades.Add(new GradeEmail
                {
                    GradeValue = grade.Value,
                    CourseDomain = existingCourse.Domain.ToString(),
                    DateOfGrade = grade.DateOfGrade
                });
            }
            
            var queue = _configuration.GetSection("RabbitMqQueues")["StudentGradeDataDistributionQueue"];

            if (string.IsNullOrEmpty(queue))
            {
                throw new EntityNotFoundException("Could not find specified queue!");
            }

            if (studentGradeEmail.Grades.IsNullOrEmpty() || string.IsNullOrEmpty(studentGradeEmail.StudentEmail))
            {
                continue;
            }
            
            _messagePublisher.PublishMessage(studentGradeEmail, queue);
        }
    }
}