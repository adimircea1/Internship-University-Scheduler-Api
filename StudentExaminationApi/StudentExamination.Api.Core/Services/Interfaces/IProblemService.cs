using System.Linq.Expressions;
using StudentExamination.Api.Core.Models.ExaminationModels;

namespace StudentExamination.Api.Core.Services.Interfaces;

public interface IProblemService
{
    public Task<Problem?> GetProblemByQueryAsync(Expression<Func<Problem, bool>> query);
    public Task<Problem> GetProblemByIdAsync(int id);
    public Task<List<Problem>> GetProblemsByQueryAsync(Expression<Func<Problem, bool>> query);
    public Task<List<Problem>> GetAllProblemsAsync();
    public Task<List<AnswerOption>> GetAllAnswerOptionsForProblemWithId(int problemId);
    public Task<List<CorrectAnswer>> GetAllCorrectAnswersForProblemWithId(int problemId);
    public Task QueueAddProblemAsync(Problem problem);
    public Task QueueAddProblemsAsync(List<Problem> problems);
    public Task QueueUpdateProblemByIdAsync(int id, Problem problem, string problemJson);
    public Task QueueDeleteProblemByIdAsync(int id);
    public void QueueDeleteAllProblems();
    public Task AddProblemAsync(Problem problem);
    public Task AddProblemsAsync(List<Problem> problems);
    public Task UpdateProblemByIdAsync(int id, Problem problem, string problemJson);
    public Task DeleteProblemByIdAsync(int id);
    public Task DeleteAllProblemsAsync();
}