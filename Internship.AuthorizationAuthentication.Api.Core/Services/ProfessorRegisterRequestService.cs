using Internship.AuthorizationAuthentication.Api.Core.CustomExceptions;
using Internship.AuthorizationAuthentication.Api.Core.Models.Input;
using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Services.LambdaExpressionCreator;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Services;

[Registration(Type = RegistrationKind.Scoped)]
public class ProfessorRegisterRequestService : IProfessorRegisterRequestService
{
    private readonly IDatabaseGenericRepository<ProfessorRegisterRequest> _registerRequestRepository;
    private readonly ILogger<ProfessorRegisterRequest> _logger;
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IUserService _userService;
    private readonly IServiceProvider _serviceProvider;

    public ProfessorRegisterRequestService(
        IDatabaseGenericRepository<ProfessorRegisterRequest> registerRequestRepository, 
        ILogger<ProfessorRegisterRequest> logger,
        IExpressionBuilder expressionBuilder,
        IUserService userService, 
        IServiceProvider serviceProvider)
    {
        _registerRequestRepository = registerRequestRepository;
        _logger = logger;
        _expressionBuilder = expressionBuilder;
        _userService = userService;
        _serviceProvider = serviceProvider;
    }

    public async Task<ProfessorRegisterRequest> GetRegisterRequestByIdAsync(int id)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving a professor register request by id {id} has been made!");
        return await _registerRequestRepository.GetEntityByQueryAsync(request => request.Id == id)
               ?? throw new EntityNotFoundException($"Could not find register requestService with id {id}");
    }
    
    public async Task<List<ProfessorRegisterRequest>> GetAllRegisterRequests()
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving all professor register requests has been made!");
        return await _registerRequestRepository.GetAllEntitiesAsync();
    }
    
    public async Task<DatabaseFeedback<ProfessorRegisterRequest>> GetOrderedRegisterRequestsAsync(PaginationSetting paginationSetting)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of ordered professor register requests by {paginationSetting.OrderBy} property has been made!");
        var numberOfEntitiesToSkip = (paginationSetting.PageNumber - 1) * paginationSetting.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<ProfessorRegisterRequest>(paginationSetting.OrderBy);
        return await _registerRequestRepository.GetOrderedEntitiesAsync(numberOfEntitiesToSkip, paginationSetting.PageSize, orderByExpression, paginationSetting.OrderDirection);
    }

    public async Task<DatabaseFeedback<ProfessorRegisterRequest>> GetFilteredRegisterRequestsAsync(FilteringSettings filteringSettings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered professor register requests has been made!");
        var numberOfEntitiesToSkip = (filteringSettings.PageNumber - 1) * filteringSettings.PageSize;
        var professorRegisterRequestFilter = _serviceProvider.GetRequiredService<IFilter<ProfessorRegisterRequest>>();
        return await _registerRequestRepository.GetFilteredEntitiesAsync(numberOfEntitiesToSkip, filteringSettings.PageSize, filteringSettings.FilterBy, professorRegisterRequestFilter);
    }

    public async Task<DatabaseFeedback<ProfessorRegisterRequest>> GetFilteredAndOrderedRegisterRequestsAsync(FilterOrderSettings settings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered professor register requests has been made!");
        var numberOfEntitiesToSkip = (settings.PageNumber - 1) * settings.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<ProfessorRegisterRequest>(settings.OrderBy);
        var professorRegisterRequestFilter = _serviceProvider.GetRequiredService<IFilter<ProfessorRegisterRequest>>();
        return await _registerRequestRepository.GetFilteredAndOrderedEntitiesAsync(numberOfEntitiesToSkip, settings.PageSize, orderByExpression, settings.OrderDirection, settings.FilterBy, professorRegisterRequestFilter);
    }

    public async Task<ProfessorRegisterRequest?> GetRegisterRequestByEmailAsync(string email)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving a professor register request by email {email} has been made!");
        return await _registerRequestRepository.GetEntityByQueryAsync(request => request.Email == email);
    }
    
    public async Task AddRegisterRequestAsync(ProfessorRegisterRequest requestService)
    {
        await QueueAddRegisterRequestAsync(requestService);
        await _registerRequestRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully added a new professor register request!");
    }

    public async Task DeleteRegisterRequestByIdAsync(int id)
    {
        await QueueDeleteRegisterRequestByIdAsync(id);
        await _registerRequestRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully deleted professor register request with id {id}!");
    }
    
    public async Task DeleteRegisterRequest(ProfessorRegisterRequest studentRegisterRequestService)
    {
        QueueDeleteRegisterRequest(studentRegisterRequestService);
        await _registerRequestRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully deleted professor register request!");
    }

    public async Task QueueAddRegisterRequestAsync(ProfessorRegisterRequest requestService)
    {
        //create a validation extension for register requestService
        //validate if text related fields may contain (or not) other characters than letters and white spaces
        //requestService.ValidateRequest();
        
        //verify if a register requestService was already made by using the current email
        var existingRegisterRequest = await GetRegisterRequestByEmailAsync(requestService.Email);
        if (existingRegisterRequest is not null)
        {
            throw new RegisterRequestAlreadyMadeException($"An user having email {requestService.Email} already made a register requestService!");
        }
        
        //now verify if someone tries to make a register requestService with an existing user email 
        var existingUserByEmail = await _userService.GetUserByEmailAsync(requestService.Email);
        if (existingUserByEmail is not null)
        {
            throw new UserAlreadyRegisteredException($"An user having email {existingUserByEmail.Email} already applied!");
        }
        
        await _registerRequestRepository.AddEntityAsync(requestService);
    }

    public async Task QueueDeleteRegisterRequestByIdAsync(int id)
    {
        var existentRequest = await GetRegisterRequestByIdAsync(id);
        _registerRequestRepository.DeleteEntity(existentRequest);
    }
    
    public void QueueDeleteRegisterRequest(ProfessorRegisterRequest studentRegisterRequestService)
    {
        _registerRequestRepository.DeleteEntity(studentRegisterRequestService);
    }
}