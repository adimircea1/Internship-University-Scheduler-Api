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
public class AnswerOptionService : IAnswerOptionService
{
    private readonly IDatabaseGenericRepository<AnswerOption> _problemAnswerOptionRepository;
    private readonly ILogger<AnswerOptionService> _logger;

    public AnswerOptionService(
        IDatabaseGenericRepository<AnswerOption> problemAnswerOptionRepository,
        ILogger<AnswerOptionService> logger)
    {
        _problemAnswerOptionRepository = problemAnswerOptionRepository;
        _logger = logger;
    }

    public async Task<AnswerOption?> GetAnswerOptionByQueryAsync(Expression<Func<AnswerOption, bool>> query)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving answer option by query has been made!");
        return await _problemAnswerOptionRepository.GetEntityByQueryAsync(query) ?? throw new EntityNotFoundException("Could not find answer option by specified query!");
    }

    public async Task<List<AnswerOption>> GetAnswerOptionsByQueryAsync(Expression<Func<AnswerOption, bool>> query)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving answer options by query has been made!");
        return await _problemAnswerOptionRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task AddAnswerOptionAsync(AnswerOption answerOption)
    {
        await QueueAddAnswerOptionAsync(answerOption);
        await _problemAnswerOptionRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully added answer option!");
    }

    public async Task AddMultipleAnswerOptionsAsync(List<AnswerOption> problemAnswerOptionList)
    {
        await QueueAddMultipleAnswerOptionsAsync(problemAnswerOptionList);
        await _problemAnswerOptionRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully added a list of answer options!");
    }

    public async Task UpdateAnswerOptionByIdAsync(int answerOptionId, AnswerOption answerOption, string updatedAnswerOptionJson)
    {
        await QueueUpdateAnswerOptionByIdAsync(answerOptionId, answerOption, updatedAnswerOptionJson);
        await _problemAnswerOptionRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully updated answer option with id {answerOptionId}");
    }

    public async Task DeleteAnswerOptionByIdAsync(int problemAnswerOptionId)
    {
        await QueueDeleteAnswerOptionByIdAsync(problemAnswerOptionId);
        await _problemAnswerOptionRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted answer option with id {problemAnswerOptionId}!");
    }

    public async Task DeleteAllAnswerOptionsAsync()
    {
        QueueDeleteAnswerOptionsAsync();
        await _problemAnswerOptionRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all answer options!");
    }

    public async Task QueueAddAnswerOptionAsync(AnswerOption answerOption)
    {
        answerOption.ValidateEntity();
        await _problemAnswerOptionRepository.AddEntityAsync(answerOption);
    }

    public async Task QueueAddMultipleAnswerOptionsAsync(List<AnswerOption> problemAnswerOptionList)
    {
        foreach (var problemAnswerOption in problemAnswerOptionList)
        {
            problemAnswerOption.ValidateEntity();
            await _problemAnswerOptionRepository.AddEntityAsync(problemAnswerOption);
        }
    }

    public async Task QueueUpdateAnswerOptionByIdAsync(int answerOptionId, AnswerOption answerOption, string updatedAnswerOptionJson)
    {
        var existingAnswerOption = await GetAnswerOptionByQueryAsync(answer => answer.Id == answerOptionId);
        answerOption.ValidateEntity();
        _problemAnswerOptionRepository.UpdateEntity(existingAnswerOption!, updatedAnswerOptionJson);
    }

    public async Task QueueDeleteAnswerOptionByIdAsync(int problemAnswerOptionId)
    {
        var existingProblemAnswerOption = await GetAnswerOptionByQueryAsync(problemAnswerOption => problemAnswerOption.Id == problemAnswerOptionId);
        _problemAnswerOptionRepository.DeleteEntity(existingProblemAnswerOption!);
    }

    public void QueueDeleteAnswerOptionsAsync()
    {
        _problemAnswerOptionRepository.DeleteAllEntities();
    }
}
