using OnEntitySharedLogic.Utils;

namespace StudentExamination.Api.Core.Models.ExaminationModels;

public class AnswerOption : IEntity
{
    public int Id { get; set; }
    public int ProblemId { get; set; }
    public string Answer { get; set; } = string.Empty;
    public Problem? Problem { get; set; }
}