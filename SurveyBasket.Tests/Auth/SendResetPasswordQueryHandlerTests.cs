using Microsoft.Extensions.Logging;
using SurveyBasket.BLL.Contracts.Authantication;
using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Auth.Query.SendResetPassword;
using SurveyBasket.BLL.Helpers;

namespace SurveyBasket.Tests.Auth;
public class SendResetPasswordQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<ILogger<SendResetPasswordQueryHandler>> _loggerMock;
    private readonly Mock<ISendResetPasswordEmailHelper> _emailHelperMock;
    private readonly SendResetPasswordQueryHandler _handler;

    public SendResetPasswordQueryHandlerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<SendResetPasswordQueryHandler>>();
        _emailHelperMock = new Mock<ISendResetPasswordEmailHelper>();
        _handler = new SendResetPasswordQueryHandler(
            _userRepoMock.Object,
            _loggerMock.Object,
            _emailHelperMock.Object
            );

    }

    [Fact]
    public async Task Handle_WhenValidRequest_ReturnSuccess()
    {
        var dto = new ForgetPasswordRequest("test@test.com");
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            EmailConfirmed = true
        };

        _userRepoMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _userRepoMock
            .Setup(x => x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync("Code");

        _emailHelperMock
            .Setup(x => x.sendResetPasswordEmail(user, It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(request: new SendResetPasswordQuery(dto), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }
    [Fact]
    public async Task Handle_WhenUserNotFound_ReturnSuccess()
    {
        var dto = new ForgetPasswordRequest("test@test.com");

        _userRepoMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync((ApplicationUser)null!);

        var result = await _handler.Handle(request: new SendResetPasswordQuery(dto), CancellationToken.None);

        result?.IsSuccess.Should().BeTrue();    
    }
    [Fact]
    public async Task Handle_WhenEmailNotConfirmed_ReturnFailure()
    {
        var dto = new ForgetPasswordRequest("test@test.com");
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            EmailConfirmed = false
        };

        _userRepoMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        var result = await _handler.Handle(request: new SendResetPasswordQuery(dto), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.EmailNotConfirmed);
    }

    [Fact]
    public async Task Handle_Always_CallsSendResetPasswordEmailOnce()
    {
        var dto = new ForgetPasswordRequest("test@test.com");
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            EmailConfirmed = true
        };

        _userRepoMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _userRepoMock
            .Setup(x => x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync("Code");

        _emailHelperMock
            .Setup(x => x.sendResetPasswordEmail(user, It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(request: new SendResetPasswordQuery(dto), CancellationToken.None);

        _emailHelperMock.Verify(
            x=>x.sendResetPasswordEmail(user,It.IsAny<string>()),
            Times.Once
            );
    }
    
}
