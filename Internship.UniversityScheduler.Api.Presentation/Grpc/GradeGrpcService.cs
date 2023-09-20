using AutoMapper;
using Grpc.Core;
using Internship.UniversityScheduler.Api.Core.Models;
using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
using Internship.UniversityScheduler.Library.DataContracts;
using Internship.UniversityScheduler.Library.GrpcServiceInterfaces;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Presentation.Grpc;

[Registration(Type = RegistrationKind.Scoped)]
public class GradeGrpcService : IGradeGrpcService
{
    private readonly IGradeService _gradeService;
    private readonly IMapper _mapper;
    private readonly ILogger<GradeGrpcService> _logger;

    public GradeGrpcService(
        IGradeService gradeService,
        IMapper mapper, 
        ILogger<GradeGrpcService> logger)
    {
        _gradeService = gradeService;
        _mapper = mapper;
        _logger = logger;
    }

    public async ValueTask AddGradeAsync(GradeInputDataContract gradeInputToAdd)
    {
        try
        {
            await _gradeService.AddGradeAsync(_mapper.Map<Grade>(gradeInputToAdd));
            _logger.LogInformation($"{DateTime.Now} ---> Successfully received and added grade data from channel!");
        }
        catch (Exception ex)
        {
            var status = new Status(StatusCode.InvalidArgument, ex.Message);
            throw new RpcException(status);
        }
    } 
}