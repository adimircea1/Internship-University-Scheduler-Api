using System.Linq.Expressions;
using StudentExamination.Api.Core.Models.ExaminationModels;

namespace StudentExamination.Api.Core.Services.Interfaces;

public interface IAnswerOptionService
{
    public Task<AnswerOption?> GetAnswerOptionByQueryAsync(Expression<Func<AnswerOption, bool>> query);
    public Task<List<AnswerOption>> GetAnswerOptionsByQueryAsync(Expression<Func<AnswerOption, bool>> query);

    public Task AddAnswerOptionAsync(AnswerOption answerOption);
    public Task AddMultipleAnswerOptionsAsync(List<AnswerOption> answerOptionList);
    public Task UpdateAnswerOptionByIdAsync(int answerOptionId, AnswerOption answerOption, string updatedAnswerOptionJson);
    public Task DeleteAnswerOptionByIdAsync(int answerOptionId);
    public Task DeleteAllAnswerOptionsAsync();

    public Task QueueAddAnswerOptionAsync(AnswerOption answerOption);
    public Task QueueAddMultipleAnswerOptionsAsync(List<AnswerOption> answerOptionList);
    public Task QueueUpdateAnswerOptionByIdAsync(int answerOptionId, AnswerOption answerOption, string updatedAnswerOptionJson);
    public Task QueueDeleteAnswerOptionByIdAsync(int answerOptionId);
    public void QueueDeleteAnswerOptionsAsync();
}
