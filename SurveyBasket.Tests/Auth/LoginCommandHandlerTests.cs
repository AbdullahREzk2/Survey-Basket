using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SurveyBasket.BLL.CurrentUserService;
using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Auth.Command.Login;

namespace SurveyBasket.Tests.Auth;
public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<SignInManager<ApplicationUser>> _signinmanagerMock;
    private readonly Mock<IJwtProvider> _jwtproviderMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
    userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
);
        _signinmanagerMock = new Mock<SignInManager<ApplicationUser>>(
            userManagerMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
            null!, null!, null!, null!
        ); _userRepositoryMock = new Mock<IUserRepository>();
        _jwtproviderMock = new Mock<IJwtProvider>();
        _handler = new LoginCommandHandler(
            _userRepositoryMock.Object,
            _signinmanagerMock.Object,
            _jwtproviderMock.Object
            );
    }


    // when Login success -->
    [Fact]
    public async Task Handle_whenValidLogin_ReturnSuccess()
    {
        //arrenge
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            isDisabled = false,
        };

        _userRepositoryMock
            .Setup(x=>x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _signinmanagerMock
            .Setup(x => x.PasswordSignInAsync(user, "Anypassword", false, true))
            .ReturnsAsync(SignInResult.Success);

        _userRepositoryMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string>());

        _userRepositoryMock
            .Setup(x => x.GetUserPermissionsAsync(It.IsAny<IList<string>>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string>());

        _jwtproviderMock
            .Setup(x => x.GenerateToken(user, It.IsAny<IList<string>>(), It.IsAny<IList<string>>()))
            .Returns(("test-token",30));

        _userRepositoryMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);


        //act
        var result = await _handler.Handle(new LoginCommand("test@test.com", "Anypassword"), CancellationToken.None);

        //assert

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Token.Should().Be("test-token");
    }

    // when user not found -->
    [Fact]
    public async Task Handle_whenUserNotFound_ReturnFailed()
    {
        // arrenge 
        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser)null!);
        // act
        var result = await _handler.Handle(new LoginCommand("test@test.com", "Anypass"), CancellationToken.None);
        //assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.InvalidCredentials);
    }

    //when user disabled -->
    [Fact]
    public async Task Handle_whenUserIsDisabled_ReturnFailed()
    {
        //arrenge
        var user = new ApplicationUser
        {
            Id = "1",
            isDisabled = true,
        };

        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        // act
        var result = await _handler.Handle(new LoginCommand("test@test.com", "anyPass"), CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.DisabledUser);
    }

    // when email not confirmed -->
    [Fact]
    public async Task Handle_whenEmailNotConfirmed_ReturnFailed()
    {
        //arrenge
        var user = new ApplicationUser
        {
            Id = "1",
            isDisabled = false,
            EmailConfirmed = false
        };

        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _signinmanagerMock
            .Setup(x => x.PasswordSignInAsync(user, "AnyPass", false, true))
            .ReturnsAsync(SignInResult.NotAllowed);
        // act
        var result = await _handler.Handle(new LoginCommand("test@test.com", "AnyPass"), CancellationToken.None);

        // assert

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.EmailNotConfirmed);
    }

    // when user lockout -->
    [Fact]
    public async Task Handle_whenUserLockedOut_ReturnFailed()
    {
       //arrenge
        var user = new ApplicationUser
        {
            Id = "1",
            isDisabled = false,
            EmailConfirmed = true,
        };

        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _signinmanagerMock
            .Setup(x => x.PasswordSignInAsync(user, "AnyPass", false, true))
            .ReturnsAsync(SignInResult.LockedOut);
        // act
        var result = await _handler.Handle(new LoginCommand("test@test.com", "AnyPass"), CancellationToken.None);

        // assert

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.UserLockedOut);
    }
}
