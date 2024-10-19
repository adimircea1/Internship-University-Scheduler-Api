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
public class ProfessorGrpcService : IProfessorGrpcService
{
    private readonly IProfessorService _professorService;
    private readonly IMapper _mapper;
    private readonly ILogger<IProfessorGrpcService> _logger;

    public ProfessorGrpcService(
        IProfessorService professorService,
        IMapper mapper, 
        ILogger<IProfessorGrpcService> logger)
    {
        _professorService = professorService;
        _mapper = mapper;
        _logger = logger;
    }

    public async ValueTask AddProfessorAsync(ProfessorInputDataContract professorData)
    {
        try
        {
            await _professorService.AddProfessorAsync(_mapper.Map<Professor>(professorData));
            _logger.LogInformation($"{DateTime.Now} ---> Successfully received and added professor data from channel!");
        }
        catch (Exception ex)
        {
            var status = new Status(StatusCode.InvalidArgument, ex.Message);
            throw new RpcException(status);
        }
    }
}