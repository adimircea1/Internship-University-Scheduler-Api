using System.Linq.Expressions;
using Internship.UniversityScheduler.Library.DataContracts;
using Internship.UniversityScheduler.Library.GrpcServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.GRPC.GenericContracts;
using OnEntitySharedLogic.GRPC.Grpc_Setups;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Services.LambdaExpressionCreator;
using OnEntitySharedLogic.Utils;
using StudentExamination.Api.Core.CustomExceptions;
using StudentExamination.Api.Core.Models;
using StudentExamination.Api.Core.Models.ExaminationModels;
using StudentExamination.Api.Core.Services.Interfaces;

namespace StudentExamination.Api.Core.Services;

[Registration(Type = RegistrationKind.Scoped)]
public class ExamService : IExamService
{
    private readonly IDatabaseGenericRepository<Exam> _examRepository;
    private readonly ILogger<ExamService> _logger;
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IServiceProvider _serviceProvider;
    private readonly IProblemService _problemService;
    private readonly IExamAttendanceService _attendanceService;
    private readonly IGrpcClientService _grpcClientService;

    public ExamService(
        IDatabaseGenericRepository<Exam> examRepository,
        ILogger<ExamService> logger,
        IExpressionBuilder expressionBuilder,
        IServiceProvider serviceProvider,
        IProblemService problemService,
        IExamAttendanceService attendanceService,
        IGrpcClientService grpcClientService)
    {
        _examRepository = examRepository;
        _logger = logger;
        _expressionBuilder = expressionBuilder;
        _serviceProvider = serviceProvider;
        _problemService = problemService;
        _attendanceService = attendanceService;
        _grpcClientService = grpcClientService;
    }

    public async Task<Exam?> GetExamByQueryAsync(Expression<Func<Exam, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving an exam by query has been made");
        return await _examRepository.GetEntityByQueryAsync(query);
    }

    public async Task<Exam> GetExamByIdAsync(int id)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving exam with id {id} has been made");
        return await _examRepository.GetEntityByQueryAsync(exam => exam.Id == id) ??
               throw new EntityNotFoundException($"Couldn't find exam with id {id}");
    }

    public async Task<List<Exam>> GetExamsByQueryAsync(Expression<Func<Exam, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving exams by query has been made");
        return await _examRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task<IOrderedEnumerable<Exam>> GetAvailableExamsOfStudent(int studentId)
    {
        var attendances = await _attendanceService.GetExamAttendancesOfStudentAsync(studentId);
        var exams = new List<Exam>();
        foreach (var examAttendance in attendances)
        {
            var exam = await GetExamByIdAsync(examAttendance.ExamId);

            if (exam.AvailableFrom.ToUniversalTime() <= DateTime.UtcNow && exam.AvailableUntil.ToUniversalTime() >= DateTime.UtcNow && exam.FinalGrade == null)
            {
                exams.Add(exam);
            }
        }

        return exams.OrderBy(exam => exam.AvailableFrom);
    }

    public async Task<IOrderedEnumerable<Exam>> GetUnavailableExamsOfStudent(int studentId)
    {
        var attendances = await _attendanceService.GetExamAttendancesOfStudentAsync(studentId);
        var exams = new List<Exam>();
        foreach (var examAttendance in attendances)
        {
            var exam = await GetExamByIdAsync(examAttendance.ExamId);

            if (exam.AvailableFrom.ToUniversalTime() > DateTime.UtcNow || exam.AvailableUntil.ToUniversalTime() <  DateTime.UtcNow || exam.FinalGrade != null)
            {
                exams.Add(exam);
            }
        }

        return exams.OrderBy(exam => exam.AvailableFrom);
    }

    public async Task<IOrderedEnumerable<Exam>> GetAllExamsOfStudent(int studentId)
    {
        var attendances = await _attendanceService.GetExamAttendancesOfStudentAsync(studentId);
        var exams = new List<Exam>();
        foreach (var examAttendance in attendances)
        {
            var exam = await GetExamByIdAsync(examAttendance.ExamId);
            exams.Add(exam);
        }
        
        return exams.OrderBy(exam => exam.AvailableFrom);
    }
    
    public async Task<List<Exam>> GetAllExamsAsync()
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all exams method has been called!");
        return await _examRepository.GetAllEntitiesAsync();
    }

    public async Task<List<Problem>> GetAllProblemsFromExamAsync(int examId)
    {
        return await _problemService.GetProblemsByQueryAsync(problem => problem.ExamId == examId);
    }

    public async Task<DatabaseFeedback<Exam>> GetOrderedExamsAsync(PaginationSetting paginationSetting)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of ordered exams by {paginationSetting.OrderBy} property has been made!");
        var numberOfEntitiesToSkip = (paginationSetting.PageNumber - 1) * paginationSetting.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Exam>(paginationSetting.OrderBy);
        return await _examRepository.GetOrderedEntitiesAsync(numberOfEntitiesToSkip, paginationSetting.PageSize, orderByExpression, paginationSetting.OrderDirection);
    }

    public async Task<DatabaseFeedback<Exam>> GetFilteredExamsAsync(FilteringSettings filteringSettings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered exams has been made!");
        var numberOfEntitiesToSkip = (filteringSettings.PageNumber - 1) * filteringSettings.PageSize;
        var examFilter = _serviceProvider.GetRequiredService<IFilter<Exam>>();
        return await _examRepository.GetFilteredEntitiesAsync(numberOfEntitiesToSkip, filteringSettings.PageSize, filteringSettings.FilterBy, examFilter);
    }

    public async Task<DatabaseFeedback<Exam>> GetFilteredAndOrderedExamsAsync(FilterOrderSettings settings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered exams has been made!");
        var numberOfEntitiesToSkip = (settings.PageNumber - 1) * settings.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Exam>(settings.OrderBy);
        var examFilter = _serviceProvider.GetRequiredService<IFilter<Exam>>();
        return await _examRepository.GetFilteredAndOrderedEntitiesAsync(numberOfEntitiesToSkip, settings.PageSize, orderByExpression, settings.OrderDirection, settings.FilterBy, examFilter);
    }

    public async Task AddExamAsync(Exam exam)
    {
        await QueueAddExamAsync(exam);
        await _examRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a new exam!");
    }

    public async Task AddExamsAsync(List<Exam> exams)
    {
        await QueueAddExamsAsync(exams);
        await _examRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a list of exams!");
    }

    public async Task UpdateExamByIdAsync(int id, Exam exam, string examJson)
    {
        await QueueUpdateExamByIdAsync(id, exam, examJson);
        await _examRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully updated exam with id {id}!");
    }

    public async Task DeleteExamByIdAsync(int id)
    {
        await QueueDeleteExamByIdAsync(id);
        await _examRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted a exam having the id {id}!");
    }

    public async Task DeleteAllExamsAsync()
    {
        QueueDeleteAllExams();
        await _examRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all exams!");
    }
    
    public async Task GradeExamAsync(int examId, int studentId, List<ProvidedAnswers> answers)
    {
        await QueueGradeExamAsync(examId, studentId, answers);
        await _examRepository.SaveChangesAsync();
    }

    public async Task QueueAddExamAsync(Exam exam)
    {
        exam.ValidateEntity();

        var courseService = _grpcClientService.GetService<ICourseGrpcService>();
        var existingCourse = await courseService.GetCourseDataAsync(new SimpleValueContract<int> { Value = exam.CourseId });

        await _examRepository.AddEntityAsync(exam);
    }

    public async Task QueueAddExamsAsync(List<Exam> exams)
    {
        var courseService = _grpcClientService.GetService<ICourseGrpcService>();
        foreach (var exam in exams)
        {
            exam.ValidateEntity();
            var existingCourse = await courseService.GetCourseDataAsync(new SimpleValueContract<int> { Value = exam.CourseId });
            await _examRepository.AddEntityAsync(exam);
        }
    }

    public async Task QueueUpdateExamByIdAsync(int id, Exam exam, string examJson)
    {
        var existingExam = await GetExamByIdAsync(id);
        exam.ValidateEntity();
        _examRepository.UpdateEntity(existingExam, examJson);
    }

    public async Task QueueDeleteExamByIdAsync(int id)
    {
        var existingExam = await GetExamByIdAsync(id);
        _examRepository.DeleteEntity(existingExam);
    }

    public async Task<object> GenerateExamAsync(int examId, int studentId)
    {
        var existingExam = await GetExamByIdAsync(examId);

        // if (existingExam.FinalGrade is not null)
        // {
        //     throw new DuplicateExamGenerationException($"Cannot generate exam with id {existingExam.Id} twice!");
        // }

        var examProblems = await GetAllProblemsFromExamAsync(existingExam.Id);

        if (examProblems.IsNullOrEmpty())
        {
            throw new EmptyExamException($"The exam with id {existingExam.Id} should contain at least 1 problem!");
        }

        var studentService = _grpcClientService.GetService<IStudentGrpcService>();
        var studentData = await studentService.GetStudentDataAsync(new SimpleValueContract<int> { Value = studentId });

        var courseService = _grpcClientService.GetService<ICourseGrpcService>();
        var courseData = await courseService.GetCourseDataAsync(new SimpleValueContract<int> { Value = existingExam.CourseId });


        _logger.LogInformation($"{DateTime.Now} ---> Successfully generated exam with id {existingExam.Id}");

        return new
        {
            ExamId = existingExam.Id,
            CourseDomain = courseData.Domain.ToString(),
            StudentName = studentData.FullName,
            ExamTime = existingExam.ExamDuration,
            Problems = examProblems
        };
    }

    public void QueueDeleteAllExams()
    {
        _examRepository.DeleteAllEntities();
    }

    public async Task QueueGradeExamAsync(int examId, int studentId, List<ProvidedAnswers> answers)
    {
        var existingExam = await GetExamByIdAsync(examId);
        existingExam.FinalGrade = await GetFinalGradeAsync(answers, existingExam.PartialGradingAllowed);
        await AddGradeToStudentAsync(existingExam, studentId);
        _logger.LogInformation($"{DateTime.Now} ---> Successfully graded exam with id {existingExam.Id}");
    }

    public async Task<ExamRemainingTime> GetExamRemainingTimeAsync(int examId)
    {
        var existingExam = await GetExamByIdAsync(examId);
        if (existingExam.FirstVisitTime is null)
        {
            existingExam.FirstVisitTime = DateTime.UtcNow;
            await _examRepository.SaveChangesAsync();
        }

        var currentTime = DateTime.UtcNow;
        var elapsedTime = currentTime - existingExam.FirstVisitTime;

        var remainingTime = TimeSpan.FromMinutes(existingExam.ExamDuration) - elapsedTime;

        if (remainingTime.Value.TotalSeconds <= 0)
        {
            return new ExamRemainingTime()
            {
                RemainingMinutes = 0,
                RemainingSeconds = 0
            };
        }

        return new ExamRemainingTime
        {
            RemainingMinutes = (int)remainingTime.Value.TotalMinutes,
            RemainingSeconds = remainingTime.Value.Seconds
        };
    }
    
    private async Task<int> GetFinalGradeAsync(List<ProvidedAnswers> answers, bool partialGradingAllowed)
    {
        var totalPoints = 0.00;
        foreach (var providedAnswer in answers)
        {
            var examProblem = await _problemService.GetProblemByIdAsync(providedAnswer.ProblemId);
            var correctProblemAnswers = await _problemService.GetAllCorrectAnswersForProblemWithId(examProblem.Id);
            var answerValues = correctProblemAnswers.Select(correctProblemAnswer => correctProblemAnswer.Answer);

            totalPoints += examProblem.ProblemType switch
            {
                ProblemType.SingleAnswer => CalculateSingleAnswerPoints(examProblem.CorrectAnswers, providedAnswer.Answers, examProblem.Points),
                ProblemType.MultipleAnswer => CalculateMultipleAnswerPoints(examProblem.CorrectAnswers, providedAnswer.Answers, examProblem.Points, partialGradingAllowed),
                ProblemType.TextAnswer => CalculateTextAnswerPoints(examProblem.CorrectAnswers, providedAnswer.Answers, examProblem.Points),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return totalPoints % 1 >= 0.5 ? (int)Math.Ceiling(totalPoints) : (int)Math.Floor(totalPoints);
    }

    private static int CalculateSingleAnswerPoints(IReadOnlyList<CorrectAnswer> problemCorrectAnswers, IReadOnlyList<string> answersOfProblem, int points)
    {
        if (answersOfProblem.Count == 0)
        {
            return 0;
        }

        return problemCorrectAnswers[0].Answer == answersOfProblem[0] ? points : 0;
    }

    private static double CalculateMultipleAnswerPoints(IReadOnlyCollection<CorrectAnswer> problemCorrectAnswers, IReadOnlyCollection<string> answersOfProblem, int points, bool partialGradingAllowed)
    {
        if (answersOfProblem.Count == 0)
        {
            return 0;
        }

        var correctAnswerValues = problemCorrectAnswers.Select(problemAnswer => problemAnswer.Answer);

        var numberOfCorrectAnswersSelected = answersOfProblem.Count(correctAnswerValues.Contains);
        var numberOfCorrectAnswers = problemCorrectAnswers.Count;

        switch (partialGradingAllowed)
        {
            case true:
            {
                var numberOfWrongAnswersSelected = answersOfProblem.Count - numberOfCorrectAnswersSelected;
                var pointsPerAnswer = (double)points / numberOfCorrectAnswers;

                if (numberOfCorrectAnswersSelected < numberOfWrongAnswersSelected)
                {
                    return 0;
                }

                var numberOfNotRoundedPointsEarned = pointsPerAnswer * (numberOfCorrectAnswersSelected - numberOfWrongAnswersSelected);
                return numberOfNotRoundedPointsEarned;
            }
            case false:
            {
                if (answersOfProblem.Count == 0)
                {
                    return 0;
                }

                return numberOfCorrectAnswersSelected < numberOfCorrectAnswers ? 0 : points;
            }
        }
    }

    private static int CalculateTextAnswerPoints(IReadOnlyList<CorrectAnswer> problemCorrectAnswers, IReadOnlyCollection<string> answersOfProblem, int points)
    {
        if (answersOfProblem.Count == 0)
        {
            return 0;
        }

        return answersOfProblem.Any(answerOfProblem => string
            .Compare(problemCorrectAnswers[0].Answer, answerOfProblem, StringComparison.InvariantCultureIgnoreCase) == 0)
            ? points
            : 0;
    }

    private async Task AddGradeToStudentAsync(Exam exam, int studentId)
    {
        var studentGrpcService = _grpcClientService.GetService<IStudentGrpcService>();
        var existingStudent = await studentGrpcService.GetStudentDataAsync(new SimpleValueContract<int> { Value = studentId });

        var groupIdOfStudent = existingStudent.UniversityGroupId ?? throw new EntityNotFoundException($"Student with id {existingStudent.Id} does not exist in any groups!");

        var catalogueGrpcService = _grpcClientService.GetService<ICatalogueGrpcService>();
        var existingCatalogue = await catalogueGrpcService.GetCatalogueDataAsync(new SimpleValueContract<int> { Value = groupIdOfStudent });

        var gradeGrpcService = _grpcClientService.GetService<IGradeGrpcService>();

        var gradeToAdd = new GradeInputDataContract
        {
            StudentId = existingStudent.Id,
            CourseId = exam.CourseId,
            CatalogueId = existingCatalogue.Id,
            Value = (int)exam.FinalGrade!,
            DateOfGrade = DateTime.UtcNow
        };

        await gradeGrpcService.AddGradeAsync(gradeToAdd);
    }
}