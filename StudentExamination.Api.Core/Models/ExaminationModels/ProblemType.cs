using System.Text.Json.Serialization;

namespace StudentExamination.Api.Core.Models.ExaminationModels;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProblemType
{
    SingleAnswer = 1,
    MultipleAnswer = 2,
    TextAnswer = 3
}