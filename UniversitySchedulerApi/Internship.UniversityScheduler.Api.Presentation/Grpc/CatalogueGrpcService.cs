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
public class CatalogueGrpcService : ICatalogueGrpcService
{
    private readonly ICatalogueService _catalogueService;
    private readonly IMapper _mapper;
    private readonly ILogger<CatalogueGrpcService> _logger;

    public CatalogueGrpcService(
        ICatalogueService catalogueService,
        IMapper mapper,
        ILogger<CatalogueGrpcService> logger)
    {
        _catalogueService = catalogueService;
        _mapper = mapper;
        _logger = logger;
    }

    public async ValueTask<CatalogueDataContract> GetCatalogueDataAsync(SimpleValueContract<int> catalogueId)
    {
        try
        {
            var existingCatalogue = await _catalogueService.GetCatalogueByIdAsync(catalogueId.Value);
            _logger.LogInformation($"{DateTime.Now} ---> Successfully sent catalogue with id {catalogueId.Value} data through channel!");
            return _mapper.Map<CatalogueDataContract>(existingCatalogue);
        }
        catch (EntityNotFoundException ex)
        {
            var status = new Status(StatusCode.NotFound, ex.Message);
            throw new RpcException(status);
        }
    }
    
    public async ValueTask<CatalogueDataContract> GetCatalogueDataByGroupAsync(SimpleValueContract<int> groupId)
    {
        try
        {
            var existingCatalogue = await _catalogueService.GetCatalogueByUniversityGroupIdAsync(groupId.Value);
            _logger.LogInformation($"{DateTime.Now} ---> Successfully sent catalogue with id {existingCatalogue.Id} data through channel!");
            return _mapper.Map<CatalogueDataContract>(existingCatalogue);
        }
        catch (EntityNotFoundException ex)
        {
            var status = new Status(StatusCode.NotFound, ex.Message);
            throw new RpcException(status);
        }
    }
}