using Internship.AuthorizationAuthentication.Api.Core.Models.Enums;

namespace Internship.AuthorizationAuthentication.Api.Core.Models.Dtos.PatchDtos;

public class UserUpdatedInputDto
{
    public UserType Role { get; set; }
}