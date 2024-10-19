using System.Linq.Expressions;
using OnEntitySharedLogic.Models;
using StudentExamination.Api.Core.Models;
using StudentExamination.Api.Core.Models.ExaminationModels;

namespace StudentExamination.Api.Core.Services.Interfaces;

public interface IExamService
{
    public Task<Exam?> GetExamByQueryAsync(Expression<Func<Exam, bool>> query);
    public Task<Exam> GetExamByIdAsync(int id);
    public Task<List<Exam>> GetExamsByQueryAsync(Expression<Func<Exam, bool>> query);
    public Task<List<Exam>> GetAllExamsAsync();
    public Task<List<Problem>> GetAllProblemsFromExamAsync(int examId);
    Task<DatabaseFeedback<Exam>> GetOrderedExamsAsync(PaginationSetting paginationSetting);
    Task<DatabaseFeedback<Exam>> GetFilteredExamsAsync(FilteringSettings filteringSettings);
    Task<DatabaseFeedback<Exam>> GetFilteredAndOrderedExamsAsync(FilterOrderSettings settings);
    public Task<ExamRemainingTime> GetExamRemainingTimeAsync(int examId);
    public Task<IOrderedEnumerable<Exam>> GetAvailableExamsOfStudent(int studentId);
    public Task<IOrderedEnumerable<Exam>> GetUnavailableExamsOfStudent(int studentId);
    public Task<IOrderedEnumerable<Exam>> GetAllExamsOfStudent(int studentId);
    public Task<object> GenerateExamAsync(int examId, int studentId);
    
    public Task QueueAddExamAsync(Exam exam);
    public Task QueueAddExamsAsync(List<Exam> exams);
    public Task QueueUpdateExamByIdAsync(int id, Exam exam, string examJson);
    public Task QueueDeleteExamByIdAsync(int id);
    public void QueueDeleteAllExams();
    public Task QueueGradeExamAsync(int examId, int studentId, List<ProvidedAnswers> answers);
    public Task AddExamAsync(Exam exam);
    public Task AddExamsAsync(List<Exam> exams);
    public Task UpdateExamByIdAsync(int id, Exam exam, string examJson);
    public Task DeleteExamByIdAsync(int id);
    public Task DeleteAllExamsAsync();
    public Task GradeExamAsync(int examId, int studentId, List<ProvidedAnswers> answers);
}