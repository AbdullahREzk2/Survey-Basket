using Microsoft.Extensions.Logging;
using SurveyBasket.BLL.Contracts.Authantication;
using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Auth.Query.ResendConfimationEmail;
using SurveyBasket.BLL.Helpers;

namespace SurveyBasket.Tests.Auth;
public class ResendConfirmationEmailQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<ResendConfirmationEmailQueryHandler>> _loggerMock;
    private readonly Mock<ISendConfirmationEmailHelper> _emailHelperMock;
    private readonly ResendConfirmationEmailQueryHandler _handler;

    public ResendConfirmationEmailQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<ResendConfirmationEmailQueryHandler>>();
        _emailHelperMock = new Mock<ISendConfirmationEmailHelper>();

        _handler = new ResendConfirmationEmailQueryHandler(
            _userRepositoryMock.Object,
            _loggerMock.Object,
            _emailHelperMock.Object
            );
    }


    [Fact]
    public async Task Handle_WhenValidResend_ReturnSuccess()
    {
        // arrenge  
        var DTO = new ResendConfirmationEmailRequest("test@test.com");

        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            EmailConfirmed = false
        };
        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.GenerateEmailConfirmationTokenAsync(user))
            .ReturnsAsync("Code");


        _emailHelperMock
            .Setup(x => x.sendConfirmationEmail(user, It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        //act
        var result = await _handler.Handle(new ResendConfirmationEmailQuery(DTO), CancellationToken.None);
        //assert
        result.IsSuccess.Should().BeTrue();

    }
    [Fact]
    public async Task Handle_WhenUserNotFound_ReturnSuccess()
    {
        // arrenge
        var DTO = new ResendConfirmationEmailRequest("test@test.com");

        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync((ApplicationUser)null!);
        //act
        var result = await _handler.Handle(new ResendConfirmationEmailQuery(DTO), CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
    }
    [Fact]
    public async Task Handle_WhenEmailAlreadyConfirmed_ReturnFailure()
    {
        // arrenge  
        var DTO = new ResendConfirmationEmailRequest("test@test.com");

        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            EmailConfirmed = true
        };

        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);
        //act
        var result = await _handler.Handle(new ResendConfirmationEmailQuery(DTO), CancellationToken.None);
        //assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.DuplicateConfirmation);
    }
    [Fact]
    public async Task Handle_Always_CallsSendConfirmationEmailOnce()
    {

        // arrenge  
        var DTO = new ResendConfirmationEmailRequest("test@test.com");

        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            EmailConfirmed = false
        };
        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.GenerateEmailConfirmationTokenAsync(user))
            .ReturnsAsync("Code");


        _emailHelperMock
            .Setup(x => x.sendConfirmationEmail(user, It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        //act
        var result = await _handler.Handle(new ResendConfirmationEmailQuery(DTO), CancellationToken.None);
        //assert
        _emailHelperMock.Verify(
            x=>x.sendConfirmationEmail(user,It.IsAny<string>()),
            Times.Once
            );
    }
}
