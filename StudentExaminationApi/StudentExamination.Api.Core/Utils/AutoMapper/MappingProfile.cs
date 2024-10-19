using AutoMapper;
using StudentExamination.Api.Core.Models.ExaminationModels;
using StudentExamination.Api.Core.Models.ModelDtos.ExamAttendanceDtos;
using StudentExamination.Api.Core.Models.ModelDtos.ExamDtos;
using StudentExamination.Api.Core.Models.ModelDtos.ProblemAnswerDtos;
using StudentExamination.Api.Core.Models.ModelDtos.ProblemDtos;

namespace StudentExamination.Api.Core.Utils.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //---> for get requests
        CreateMap<Exam, ExamDto>();
        CreateMap<Problem, ProblemDto>();
        CreateMap<CorrectAnswer, CorrectAnswerDto>();
        CreateMap<AnswerOption, AnswerOptionDto>();
        CreateMap<ExamAttendance, ExamAttendanceDto>();

        //---> for post requests
        CreateMap<ExamInputDto, Exam>();
        CreateMap<ProblemInputDto, Problem>();
        CreateMap<CorrectAnswerInputDto, CorrectAnswer>();
        CreateMap<AnswerOptionInputDto, AnswerOption>();
        CreateMap<ExamAttendanceInputDto, ExamAttendance>();

        //---> for patch requests
        CreateMap<ExamUpdatedInputDto, Exam>();
        CreateMap<ProblemUpdatedInputDto, Problem>();
        CreateMap<CorrectAnswerUpdatedInputDto, CorrectAnswer>();
        CreateMap<AnswerOptionUpdatedInputDto, AnswerOption>();
    }
}