using AutoMapper;
using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using Internship.AuthorizationAuthentication.Api.Core.Models.Dtos.GetDtos;
using Internship.AuthorizationAuthentication.Api.Core.Models.Dtos.PatchDtos;
using Internship.AuthorizationAuthentication.Api.Core.Models.Dtos.PostDtos;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;

namespace Internship.AuthorizationAuthentication.Api.Core.Utils.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile(IPasswordManager passwordManager)
    {
        //---> for get requests
        CreateMap<User, UserDto>();

        //---> for post requests
        CreateMap<UserInputDto, User>()
            .AfterMap((source, destination) =>
            {
                var hashedData = passwordManager.HashPassword(source.Password);
                destination.HashedPassword = hashedData.HashedPassword;
                destination.PasswordSalt = hashedData.PasswordSalt;
            });
        
        //---> for patch requests
        CreateMap<UserUpdatedInputDto, User>();
    }
}