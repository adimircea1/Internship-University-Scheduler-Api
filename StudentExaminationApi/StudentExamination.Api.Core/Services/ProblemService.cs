using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;
using StudentExamination.Api.Core.Models.ExaminationModels;
using StudentExamination.Api.Core.Services.Interfaces;

namespace StudentExamination.Api.Core.Services;

[Registration(Type = RegistrationKind.Scoped)]
public class ProblemService : IProblemService
{
    private readonly IDatabaseGenericRepository<Problem> _problemRepository;
    private readonly ILogger<Problem> _logger;
    private readonly ICorrectAnswerService _correctAnswerService;
    private readonly IAnswerOptionService _answerOptionService;

    public ProblemService(
        IDatabaseGenericRepository<Problem> problemRepository,
        ILogger<Problem> logger, 
        ICorrectAnswerService correctAnswerService,
        IAnswerOptionService answerOptionService)
    {
        _problemRepository = problemRepository;
        _logger = logger;
        _correctAnswerService = correctAnswerService;
        _answerOptionService = answerOptionService;
    }

    public async Task<Problem?> GetProblemByQueryAsync(Expression<Func<Problem, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving problem by query has been made");
        return await _problemRepository.GetEntityByQueryAsync(query);
    }

    public async Task<Problem> GetProblemByIdAsync(int id)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving problem with id {id} has been made");
        return await _problemRepository.GetEntityByQueryAsync(problem => problem.Id == id) ??
               throw new EntityNotFoundException($"Couldn't find problem with id {id}");
    }

    public async Task<List<Problem>> GetProblemsByQueryAsync(Expression<Func<Problem, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving problems by query has been made");
        return await _problemRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task<List<Problem>> GetAllProblemsAsync()
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all problems method has been called!");
        return await _problemRepository.GetAllEntitiesAsync();
    }

    public async Task<List<AnswerOption>> GetAllAnswerOptionsForProblemWithId(int problemId)
    {
        return await _answerOptionService.GetAnswerOptionsByQueryAsync(option => option.ProblemId == problemId);
    }

    public async Task<List<CorrectAnswer>> GetAllCorrectAnswersForProblemWithId(int problemId)
    {
        return await _correctAnswerService.GetCorrectAnswersByQueryAsync(answer => answer.ProblemId == problemId);
    }

    public async Task AddProblemAsync(Problem problem)
    {
        await QueueAddProblemAsync(problem);
        await _problemRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a problem!");
    }

    public async Task AddProblemsAsync(List<Problem> problems)
    {
        await QueueAddProblemsAsync(problems);
        await _problemRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a list of problems!");
    }

    public async Task UpdateProblemByIdAsync(int id, Problem problem, string problemJson)
    {
        await QueueUpdateProblemByIdAsync(id, problem, problemJson);
        await _problemRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully updated problem with id {id}!");
    }

    public async Task DeleteProblemByIdAsync(int id)
    {
        await QueueDeleteProblemByIdAsync(id);
        await _problemRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted a problem having the id {id}!");
    }

    public async Task DeleteAllProblemsAsync()
    {
        QueueDeleteAllProblems();
        await _problemRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all problems!");
    }

    public async Task QueueAddProblemAsync(Problem problem)
    {
        problem.ValidateEntity();
        await _problemRepository.AddEntityAsync(problem);
    }

    public async Task QueueAddProblemsAsync(List<Problem> problems)
    {
        foreach (var problem in problems)
        {
            problem.ValidateEntity();
            await _problemRepository.AddEntityAsync(problem);
        }
    }

    public async Task QueueUpdateProblemByIdAsync(int id, Problem problem, string problemJson)
    {
        var existingProblem = await GetProblemByIdAsync(id);
        problem.ValidateEntity();
        _problemRepository.UpdateEntity(existingProblem, problemJson);
    }

    public async Task QueueDeleteProblemByIdAsync(int id)
    {
        var existingProblem = await GetProblemByIdAsync(id);
        _problemRepository.DeleteEntity(existingProblem);
    }

    public void QueueDeleteAllProblems()
    {
        _problemRepository.DeleteAllEntities();
    }
}