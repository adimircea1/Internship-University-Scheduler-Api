using System.Linq.Expressions;
using Internship.UniversityScheduler.Library.GrpcServiceInterfaces;
using Microsoft.Extensions.Logging;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.GRPC.GenericContracts;
using OnEntitySharedLogic.GRPC.Grpc_Setups;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;
using StudentExamination.Api.Core.Models.ExaminationModels;
using StudentExamination.Api.Core.Services.Interfaces;

namespace StudentExamination.Api.Core.Services;

[Registration(Type = RegistrationKind.Scoped)]
public class ExamAttendanceService : IExamAttendanceService
{
    private readonly IDatabaseGenericRepository<ExamAttendance> _examAttendanceRepository;
    private readonly ILogger<ExamAttendanceService> _logger;
    private readonly IGrpcClientService _grpcClientService;

    public ExamAttendanceService(
        IDatabaseGenericRepository<ExamAttendance> examAttendanceRepository,
        ILogger<ExamAttendanceService> logger,
        IGrpcClientService grpcClientService)
    {
        _examAttendanceRepository = examAttendanceRepository;
        _logger = logger;
        _grpcClientService = grpcClientService;
    }

    public async Task<ExamAttendance> GetExamAttendanceByQueryAsync(Expression<Func<ExamAttendance, bool>> query)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving exam attendance by query has been made!");
        return await _examAttendanceRepository.GetEntityByQueryAsync(query) ?? throw new EntityNotFoundException("Could not find exam attendance by specified query!");
    }

    public async Task<List<ExamAttendance>> GetExamAttendancesByQueryAsync(Expression<Func<ExamAttendance, bool>> query)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving exam attendances by query has been made!");
        return await _examAttendanceRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task<List<ExamAttendance>> GetExamAttendancesOfStudentAsync(int studentId)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving exam attendances for student with id {studentId} has been made!");
        return await _examAttendanceRepository.GetEntitiesByQueryAsync(attendance => attendance.StudentId == studentId);
    }
    
    public async Task<List<ExamAttendance>> GetAttendancesOfExamAsync(int examId)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving attendances for exam with id {examId} has been made!");
        return await _examAttendanceRepository.GetEntitiesByQueryAsync(attendance => attendance.ExamId == examId);
    }
    
    
    public async Task AddExamAttendanceAsync(ExamAttendance examAttendance)
    {
        await QueueAddExamAttendanceAsync(examAttendance);
        await _examAttendanceRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully added exam attendance!");
    }

    public async Task AddMultipleExamAttendancesAsync(List<ExamAttendance> examAttendanceList)
    {
        await QueueAddMultipleExamAttendancesAsync(examAttendanceList);
        await _examAttendanceRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully added a list of exam attendances!");
    }

    public async Task DeleteExamAttendanceByIdAsync(int examAttendanceId)
    {
        await QueueDeleteExamAttendanceByIdAsync(examAttendanceId);
        await _examAttendanceRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted exam attendance with id {examAttendanceId}!");
    }

    public async Task DeleteAllExamAttendancesAsync()
    {
        QueueDeleteExamAttendancesAsync();
        await _examAttendanceRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all exam attendances!");
    }

    public async Task QueueAddExamAttendanceAsync(ExamAttendance examAttendance)
    {
        examAttendance.ValidateEntity();
        var studentService = _grpcClientService.GetService<IStudentGrpcService>();
        var existingStudent = await studentService.GetStudentDataAsync(new SimpleValueContract<int> { Value = examAttendance.StudentId });
        await _examAttendanceRepository.AddEntityAsync(examAttendance);
    }

    public async Task QueueAddMultipleExamAttendancesAsync(List<ExamAttendance> examAttendanceList)
    {
        var studentService = _grpcClientService.GetService<IStudentGrpcService>();
        foreach (var examAttendance in examAttendanceList)
        {
            examAttendance.ValidateEntity();
            var existingStudent = await studentService.GetStudentDataAsync(new SimpleValueContract<int> { Value = examAttendance.StudentId });
            await _examAttendanceRepository.AddEntityAsync(examAttendance);
        }
    }

    public async Task QueueDeleteExamAttendanceByIdAsync(int examAttendanceId)
    {
        var existingExamAttendance = await GetExamAttendanceByQueryAsync(examAttendance => examAttendance.Id == examAttendanceId);
        _examAttendanceRepository.DeleteEntity(existingExamAttendance!);
    }

    public void QueueDeleteExamAttendancesAsync()
    {
        _examAttendanceRepository.DeleteAllEntities();
    }
}
