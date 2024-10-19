using AutoMapper;
using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using Internship.AuthorizationAuthentication.Api.Core.Models.Dtos.GetDtos;
using Internship.AuthorizationAuthentication.Api.Core.Models.Dtos.PatchDtos;
using Internship.AuthorizationAuthentication.Api.Core.Models.Dtos.PostDtos;
using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using OnEntitySharedLogic.Models;

namespace Internship.AuthorizationAuthentication.Api.Presentation.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{ 
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(
        IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    //[WithClaimsAuthorization("Admin")]
    [HttpPost("user")]
    public async Task<ActionResult> AddUserAsync([FromBody] UserInputDto user)
    {
        await _userService.AddUserAsync(_mapper.Map<User>(user));
        return Ok("Successfully added a new user!");
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPost]
    public async Task<ActionResult> AddUsersAsync([FromBody] List<UserInputDto> users)
    {
        await _userService.AddUsersAsync(users
            .Select(user => _mapper.Map<User>(user))
            .ToList());
        return Ok("Successfully added a list of users!");
    }


    [Authorize]
    [HttpGet("user")]
    public async Task<ActionResult<UserDto>> GetUserByIdClaimAsync()
    {
        return Ok(_mapper.Map<UserDto>(await _userService.GetUserByIdClaimAsync()));
    }

    [WithClaimsAuthorization("Admin")]
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllUsersAsync()
    {
        return Ok((await _userService.GetAllUsersAsync())
            .Select(user => _mapper.Map<UserDto>(user))
            .ToList());
    }

    [Authorize]
    [HttpGet("role")]
    public ActionResult<string> GetUserRole()
    {
        return Ok(_userService.GetUserRoleClaim());
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPost("ordered")]
    public async Task<ActionResult<DatabaseFeedback<UserDto>>> GetOrderedUsersAsync([FromBody] PaginationSetting paginationSetting)
    {
        var databaseFeedback = await _userService.GetOrderedUsersAsync(paginationSetting);
        return Ok(new DatabaseFeedback<UserDto>
        {
            Entities = databaseFeedback.Entities.Select(professor => _mapper.Map<UserDto>(professor)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPost("filtered")]
    public async Task<ActionResult<DatabaseFeedback<UserDto>>> GetFilteredUsersAsync([FromBody] FilteringSettings filteringSettings)
    {
        var filteredUsers = await _userService.GetFilteredUsersAsync(filteringSettings);
        return Ok(new DatabaseFeedback<UserDto>
        {
            Entities = filteredUsers.Entities.Select(professor => _mapper.Map<UserDto>(professor)).ToList(),
            NumberOfEntities = filteredUsers.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPost("filtered-ordered")]
    public async Task<ActionResult<DatabaseFeedback<UserDto>>> GetFilteredAndOrderedUserAsync([FromBody] FilterOrderSettings settings)
    {
        var databaseFeedback = await _userService.GetFilteredAndOrderedUsersAsync(settings);
        return Ok(new DatabaseFeedback<UserDto>
        {
            Entities = databaseFeedback.Entities.Select(course => _mapper.Map<UserDto>(course)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateUserByIdAsync([FromRoute] int id, [FromBody] UserUpdatedInputDto user)
    {
        var userJson = HttpContext.Items["RequestBody"] as string;
        await _userService.UpdateUserByIdAsync(id, _mapper.Map<User>(user), userJson!);
        return Ok($"Successfully updated user with id {id}!");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUserByIdAsync([FromRoute] int id)
    {
        await _userService.DeleteUserByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllUsersAsync()
    {
        await _userService.DeleteAllUsersAsync();
        return NoContent();
    }

    [Authorize]
    [HttpPatch("{userId:int}/new-password")]
    public async Task<ActionResult> ChangeUserPasswordAsync([FromRoute] int userId, [FromBody] ChangePasswordOptions changeOptions)
    {
        await _userService.ChangeUserPasswordAsync(userId, changeOptions.OldPassword, changeOptions.NewPassword);
        return Ok("Successfully changed password!");
    }
}