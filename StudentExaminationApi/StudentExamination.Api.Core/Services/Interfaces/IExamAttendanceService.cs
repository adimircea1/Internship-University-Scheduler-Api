using System.Linq.Expressions;
using StudentExamination.Api.Core.Models.ExaminationModels;

namespace StudentExamination.Api.Core.Services.Interfaces;

public interface IExamAttendanceService
{
    Task<ExamAttendance> GetExamAttendanceByQueryAsync(Expression<Func<ExamAttendance, bool>> query);
    Task<List<ExamAttendance>> GetExamAttendancesByQueryAsync(Expression<Func<ExamAttendance, bool>> query);
    public Task<List<ExamAttendance>> GetExamAttendancesOfStudentAsync(int studentId);
    public Task<List<ExamAttendance>> GetAttendancesOfExamAsync(int examId);


    Task AddExamAttendanceAsync(ExamAttendance examAttendance);
    Task AddMultipleExamAttendancesAsync(List<ExamAttendance> examAttendanceList);
    Task DeleteExamAttendanceByIdAsync(int examAttendanceId);
    Task DeleteAllExamAttendancesAsync();

    Task QueueAddExamAttendanceAsync(ExamAttendance examAttendance);
    Task QueueAddMultipleExamAttendancesAsync(List<ExamAttendance> examAttendanceList);
    Task QueueDeleteExamAttendanceByIdAsync(int examAttendanceId);
    void QueueDeleteExamAttendancesAsync();
}
