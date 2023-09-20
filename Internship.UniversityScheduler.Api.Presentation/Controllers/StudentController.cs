using AutoMapper;
using Internship.UniversityScheduler.Api.Core.Models;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.GetDtos;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.PatchDto;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.PostDtos;
using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using OnEntitySharedLogic.Models;

namespace Internship.UniversityScheduler.Api.Presentation.Controllers;

[ApiController]
[Route("students")]
public class StudentController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IStudentService _studentService;

    public StudentController(
        IStudentService studentService,
        IMapper mapper)
    {
        _studentService = studentService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpPost("student")]
    public async Task<ActionResult> AddStudentAsync([FromBody] StudentInputDto student)
    {
        await _studentService.AddStudentAsync(_mapper.Map<Student>(student));
        return Ok("Successfully added a student!");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPost]
    public async Task<ActionResult> AddStudentsAsync([FromBody] List<StudentInputDto> students)
    {
        await _studentService.AddStudentsAsync(students
            .Select(student => _mapper.Map<Student>(student))
            .ToList());
        return Ok("Successfully added a list of students!");
    }

    [WithClaimsAuthorization("Admin", "Student", "Professor")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentDto>> GetStudentByIdAsync([FromRoute] int id)
    {
        var existingStudent = await _studentService.GetStudentByIdAsync(id);
        return Ok(_mapper.Map<StudentDto>(existingStudent));
    }

    [WithClaimsAuthorization("Student")]
    [HttpGet("student")]
    public async Task<ActionResult<StudentDto>> GetStudentByClaimIdAsync()
    {
        return Ok(_mapper.Map<StudentDto>(await _studentService.GetStudentByClaimIdAsync()));
    }
    
    [WithClaimsAuthorization("Admin", "Student", "Professor")]
    [HttpPost("get-by-email")]
    public async Task<ActionResult<StudentDto>> GetStudentByEmailAsync([FromBody] string email)
    {
        return Ok(_mapper.Map<StudentDto>(await _studentService.GetStudentByEmailAsync(email)));
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPost("ordered")]
    public async Task<ActionResult<DatabaseFeedback<StudentDto>>> GetOrderedStudentsAsync([FromBody] PaginationSetting paginationSetting)
    {
        var databaseFeedback = await _studentService.GetOrderedStudentsAsync(paginationSetting);
        return Ok(new DatabaseFeedback<StudentDto>
        {
            Entities = databaseFeedback.Entities.Select(student => _mapper.Map<StudentDto>(student)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPost("filtered")]
    public async Task<ActionResult<DatabaseFeedback<StudentDto>>> GetFilteredStudentsAsync([FromBody] FilteringSettings filteringSettings)
    {
        var queryableStudents = await _studentService.GetFilteredStudentsAsync(filteringSettings);
        return Ok(new DatabaseFeedback<StudentDto>
        {
            Entities = queryableStudents.Entities.Select(student => _mapper.Map<StudentDto>(student)).ToList(),
            NumberOfEntities = queryableStudents.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPost("filtered-ordered")]
    public async Task<ActionResult<DatabaseFeedback<StudentDto>>> GetFilteredAndOrderedStudentsAsync([FromBody] FilterOrderSettings settings)
    {
        var databaseFeedback = await _studentService.GetFilteredAndOrderedStudentsAsync(settings);
        return Ok(new DatabaseFeedback<StudentDto>
        {
            Entities = databaseFeedback.Entities.Select(student => _mapper.Map<StudentDto>(student)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin", "Professor")]
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<StudentDto>>> GetAllStudentsAsync()
    {
        return Ok((await _studentService.GetAllStudentsAsync())
            .Select(student => _mapper.Map<StudentDto>(student))
            .ToList());
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateStudentByIdAsync([FromRoute] int id, [FromBody] StudentUpdatedInputDto student)
    {
        var studentJson = HttpContext.Items["RequestBody"] as string;
        await _studentService.UpdateStudentByIdAsync(id, _mapper.Map<Student>(student), studentJson!);
        return Ok($"Successfully updated student with id {id}!");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteStudentById([FromRoute] int id)
    {
        await _studentService.DeleteStudentByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllStudents()
    {
        await _studentService.DeleteAllStudentsAsync();
        return NoContent();
    }
}