using System.Linq.Expressions;
using StudentExamination.Api.Core.Models.ExaminationModels;

namespace StudentExamination.Api.Core.Services.Interfaces;

public interface ICorrectAnswerService
{
    public Task<CorrectAnswer?> GetCorrectAnswerByQueryAsync(Expression<Func<CorrectAnswer, bool>> query);
    public Task<List<CorrectAnswer>> GetCorrectAnswersByQueryAsync(Expression<Func<CorrectAnswer, bool>> query);

    public Task AddCorrectAnswerAsync(CorrectAnswer correctAnswer);
    public Task AddMultipleCorrectAnswersAsync(List<CorrectAnswer> correctAnswerList);
    public Task UpdateCorrectAnswerByIdAsync(int correctAnswerId, CorrectAnswer correctAnswer, string updatedCorrectAnswerJson);
    public Task DeleteCorrectAnswerByIdAsync(int correctAnswerId);
    public Task DeleteAllCorrectAnswersAsync();

    public Task QueueAddCorrectAnswerAsync(CorrectAnswer correctAnswer);
    public Task QueueAddMultipleCorrectAnswersAsync(List<CorrectAnswer> correctAnswerList);
    public Task QueueUpdateCorrectAnswerByIdAsync(int correctAnswerId, CorrectAnswer correctAnswer, string updatedCorrectAnswerJson);
    public Task QueueDeleteCorrectAnswerByIdAsync(int correctAnswerId);
    public void QueueDeleteCorrectAnswersAsync();
}
