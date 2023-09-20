using AutoMapper;
using Internship.UniversityScheduler.Api.Core.Models;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.GetDtos;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.PatchDto;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.PostDtos;
using Internship.UniversityScheduler.Library.DataContracts;

namespace Internship.UniversityScheduler.Api.Core.Utils.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //---> for get requests
        CreateMap<Student, StudentDto>();
        CreateMap<Professor, ProfessorDto>();
        CreateMap<Course, CourseDto>();
        CreateMap<Grade, GradeDto>();
        CreateMap<UniversityGroup, UniversityGroupDto>();
        CreateMap<Catalogue, CatalogueDto>();
        CreateMap<Attendance, AttendanceDto>();
        
        //Maps for gRPC get contracts
        CreateMap<Student, StudentDataContract>()
            .ForMember(destination => destination.BirthdayDate, option => option.MapFrom(source => source.BirthdayDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)));
        
        CreateMap<Catalogue, CatalogueDataContract>();

        CreateMap<Course, CourseDataContract>();

        //---> for post requests
        CreateMap<StudentInputDto, Student>()
            .AfterMap((source, destination) =>
            {
                var fullName = string.Concat(source.FirstName, " ", source.LastName);
                destination.FullName = fullName;
            });
        CreateMap<ProfessorInputDto, Professor>();
        CreateMap<CourseInputDto, Course>();
        CreateMap<CatalogueInputDto, Catalogue>();
        CreateMap<UniversityGroupInputDto, UniversityGroup>();
        CreateMap<AttendanceInputDto, Attendance>();
        CreateMap<GradeInputDto, Grade>();
        
        //---> for gRPC post requests
        CreateMap<GradeInputDataContract, Grade>();

        CreateMap<StudentInputDataContract, Student>()
            .ForMember(destination => destination.BirthdayDate, options => options.MapFrom(source => DateOnly.FromDateTime(source.BirthdayDate)))
            .AfterMap((destination, source) =>
            {
                source.FullName = string.Concat(destination.FirstName, ' ', destination.LastName);
            });  
        
        CreateMap<ProfessorInputDataContract, Professor>()
            .ForMember(destination => destination.BirthdayDate, options => options.MapFrom(source => DateOnly.FromDateTime(source.BirthdayDate)));
        
        //---> for patch requests
        CreateMap<StudentUpdatedInputDto, Student>();
        CreateMap<ProfessorUpdatedInputDto, Professor>();
        CreateMap<CourseUpdatedInputDto, Course>();
        CreateMap<CatalogueUpdatedInputDto, Catalogue>();
        CreateMap<UniversityUpdatedInputDto, UniversityGroup>();
        CreateMap<AttendanceUpdatedInputDto, Attendance>();
        CreateMap<GradeUpdatedInputDto, Grade>();
    }
}