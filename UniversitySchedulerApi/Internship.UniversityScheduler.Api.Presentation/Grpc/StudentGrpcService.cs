using AutoMapper;
using Grpc.Core;
using Internship.UniversityScheduler.Api.Core.Models;
using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
using Internship.UniversityScheduler.Library.DataContracts;
using Internship.UniversityScheduler.Library.GrpcServiceInterfaces;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.GRPC.GenericContracts;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Presentation.Grpc;

[Registration(Type = RegistrationKind.Scoped)]
public class StudentGrpcService : IStudentGrpcService
{
    private readonly IStudentService _studentService;
    private readonly IMapper _mapper;
    private readonly ILogger<StudentGrpcService> _logger;
    
    public StudentGrpcService(
        IStudentService studentService, 
        IMapper mapper, 
        ILogger<StudentGrpcService> logger)
    {
        _studentService = studentService;
        _mapper = mapper;
        _logger = logger;
    }

    public async ValueTask<StudentDataContract> GetStudentDataAsync(SimpleValueContract<int> studentId)
    {
        try
        {
            var existingStudent = await _studentService.GetStudentByIdAsync(studentId.Value);
            _logger.LogInformation($"{DateTime.Now} ---> Successfully sent student with id {studentId.Value} data through channel!");
            return _mapper.Map<StudentDataContract>(existingStudent);
        }
        catch (EntityNotFoundException ex)
        {
            var status = new Status(StatusCode.NotFound, ex.Message);
            throw new RpcException(status);
        }
    }

    public async ValueTask AddStudentAsync(StudentInputDataContract studentData)
    {
        try
        {
            await _studentService.AddStudentAsync(_mapper.Map<Student>(studentData));
            _logger.LogInformation($"{DateTime.Now} ---> Successfully received and added student data from channel!");
        }
        catch (Exception ex)
        {
            var status = new Status(StatusCode.InvalidArgument, ex.Message);
            throw new RpcException(status);
        }
    }
}