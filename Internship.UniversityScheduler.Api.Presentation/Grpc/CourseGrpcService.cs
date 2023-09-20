using AutoMapper;
using Grpc.Core;
using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
using Internship.UniversityScheduler.Library.DataContracts;
using Internship.UniversityScheduler.Library.GrpcServiceInterfaces;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.GRPC.GenericContracts;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Presentation.Grpc;

[Registration(Type = RegistrationKind.Scoped)]
public class CourseGrpcService : ICourseGrpcService
{
    private readonly ICourseService _courseService;
    private readonly IMapper _mapper;
    private readonly ILogger<StudentGrpcService> _logger;


    public CourseGrpcService(
        ICourseService courseService,
        IMapper mapper,
        ILogger<StudentGrpcService> logger)
    {
        _courseService = courseService;
        _mapper = mapper;
        _logger = logger;
    }

    public async ValueTask<CourseDataContract> GetCourseDataAsync(SimpleValueContract<int> courseId)
    {
        try
        {
            var existingCourse = await _courseService.GetCourseByIdAsync(courseId.Value);
            _logger.LogInformation($"{DateTime.Now} ---> Successfully sent course with id {courseId.Value} data through channel!");
            return _mapper.Map<CourseDataContract>(existingCourse);
        }
        catch (EntityNotFoundException ex)
        {
            var status = new Status(StatusCode.NotFound, ex.Message);
            throw new RpcException(status);
        }
    }
}