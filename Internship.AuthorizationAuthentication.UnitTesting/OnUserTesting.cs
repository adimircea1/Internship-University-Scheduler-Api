using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using Internship.AuthorizationAuthentication.Api.Core.Models.Enums;
using Internship.AuthorizationAuthentication.Api.Core.Models.Utils;
using Internship.AuthorizationAuthentication.Api.Core.Services;
using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;
using Internship.AuthorizationAuthentication.Api.Infrastructure;
using Internship.AuthorizationAuthentication.Api.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Services.LambdaExpressionCreator;

namespace Internship.AuthorizationAuthentication.UnitTesting;

public class OnUserTesting
{
    private readonly IUserService _userService;
    private readonly User _userToAdd;

    public OnUserTesting()
    {
        var mockedPasswordManager = new Mock<IPasswordManager>();
        mockedPasswordManager.Setup(m => m.HashPassword(It.IsAny<string>()))
            .Returns(new PasswordData
            {
                HashedPassword = "HashedPassword",
                PasswordSalt = "Salt"
            }); // Set up the behavior of the HashPassword method

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("TestingInMemoryDatabase")
            .Options;

        var mockServiceProvider = Mock.Of<IServiceProvider>();
        
        IDataContext dataContext = new DataContext(options);

        var mockedHttpContextAccessor = new Mock<IHttpContextAccessor>();

        _userService = new UserService(
            new DatabaseGenericRepository<User>(dataContext), 
            new Logger<UserService>(new LoggerFactory()), 
            new ExpressionBuilder(),
            mockedHttpContextAccessor.Object,
            new PasswordManager(),
            mockServiceProvider);
        
        _userToAdd = new User
        {
            UserName = "user",
            Email = "user@gmail.com",
            Role = UserType.Admin,
            HashedPassword = mockedPasswordManager.Object.HashPassword("1234admin").HashedPassword,
            PasswordSalt = "Salt"
        };
    }

    [Fact]
    public async Task Verify_If_User_Is_Added_Correctly_Async()
    {
        //this will fail if something goes wrong
        await _userService.AddUserAsync(_userToAdd);

        var existingUser = await _userService.GetUserByIdAsync(_userToAdd.Id);

        Assert.NotNull(existingUser);
    }

    [Fact]
    public async Task Verify_If_User_Is_Successfully_Being_Deleted_Async()
    {
        await _userService.AddUserAsync(_userToAdd);

        await _userService.DeleteUserByIdAsync(_userToAdd.Id);

        //if this fails then the deletion was a success
        var student = await _userService.GetUserByQueryAsync(user => user.Id == _userToAdd.Id);

        Assert.Null(student);
    }
}