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
public class CorrectAnswerService : ICorrectAnswerService
{
    private readonly IDatabaseGenericRepository<CorrectAnswer> _problemCorrectAnswerRepository;
    private readonly ILogger<CorrectAnswerService> _logger;

    public CorrectAnswerService(
        IDatabaseGenericRepository<CorrectAnswer> problemCorrectAnswerRepository,
        ILogger<CorrectAnswerService> logger)
    {
        _problemCorrectAnswerRepository = problemCorrectAnswerRepository;
        _logger = logger;
    }

    public async Task<CorrectAnswer?> GetCorrectAnswerByQueryAsync(Expression<Func<CorrectAnswer, bool>> query)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving problem correct answer by query has been made!");
        return await _problemCorrectAnswerRepository.GetEntityByQueryAsync(query) ?? throw new EntityNotFoundException("Could not find problem correct answer by specified query!");
    }

    public async Task<List<CorrectAnswer>> GetCorrectAnswersByQueryAsync(Expression<Func<CorrectAnswer, bool>> query)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving problem correct answers by query has been made!");
        return await _problemCorrectAnswerRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task AddCorrectAnswerAsync(CorrectAnswer correctAnswer)
    {
        await QueueAddCorrectAnswerAsync(correctAnswer);
        await _problemCorrectAnswerRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully added problem correct answer!");
    }

    public async Task AddMultipleCorrectAnswersAsync(List<CorrectAnswer> problemCorrectAnswerList)
    {
        await QueueAddMultipleCorrectAnswersAsync(problemCorrectAnswerList);
        await _problemCorrectAnswerRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully added a list of problem correct answers!");
    }

    public async Task UpdateCorrectAnswerByIdAsync(int correctAnswerId, CorrectAnswer correctAnswer, string updatedCorrectAnswerJson)
    {
        await QueueUpdateCorrectAnswerByIdAsync(correctAnswerId, correctAnswer, updatedCorrectAnswerJson);
        await _problemCorrectAnswerRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully updated answer option with id {correctAnswerId}");
    }

    public async Task DeleteCorrectAnswerByIdAsync(int problemCorrectAnswerId)
    {
        await QueueDeleteCorrectAnswerByIdAsync(problemCorrectAnswerId);
        await _problemCorrectAnswerRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted problem correct answer with id {problemCorrectAnswerId}!");
    }

    public async Task DeleteAllCorrectAnswersAsync()
    {
        QueueDeleteCorrectAnswersAsync();
        await _problemCorrectAnswerRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all problem correct answers!");
    }

    public async Task QueueAddCorrectAnswerAsync(CorrectAnswer correctAnswer)
    {
        correctAnswer.ValidateEntity();
        await _problemCorrectAnswerRepository.AddEntityAsync(correctAnswer);
    }

    public async Task QueueAddMultipleCorrectAnswersAsync(List<CorrectAnswer> problemCorrectAnswerList)
    {
        foreach (var problemCorrectAnswer in problemCorrectAnswerList)
        {
            problemCorrectAnswer.ValidateEntity();
            await _problemCorrectAnswerRepository.AddEntityAsync(problemCorrectAnswer);
        }
    }

    public async Task QueueUpdateCorrectAnswerByIdAsync(int correctAnswerId, CorrectAnswer correctAnswer, string updatedCorrectAnswerJson)
    {
        var existingCorrectAnswer = await GetCorrectAnswerByQueryAsync(answer => answer.Id == correctAnswerId);
        correctAnswer.ValidateEntity();
        _problemCorrectAnswerRepository.UpdateEntity(existingCorrectAnswer!, updatedCorrectAnswerJson);
    }

    public async Task QueueDeleteCorrectAnswerByIdAsync(int problemCorrectAnswerId)
    {
        var existingProblemCorrectAnswer = await GetCorrectAnswerByQueryAsync(problemCorrectAnswer => problemCorrectAnswer.Id == problemCorrectAnswerId);
        _problemCorrectAnswerRepository.DeleteEntity(existingProblemCorrectAnswer!);
    }

    public void QueueDeleteCorrectAnswersAsync()
    {
        _problemCorrectAnswerRepository.DeleteAllEntities();
    }
}
