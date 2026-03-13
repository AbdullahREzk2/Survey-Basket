using System.Text;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.BLL.Contracts.Authantication;
using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Auth.Command.ConfirmEmail;
using SurveyBasket.BLL.Helpers;
using SurveyBasket.DAL.seedData;

namespace SurveyBasket.Tests.Auth;
public class ConfirmEmailCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IBackgroundJobClient> _backgroundJobMock;
    private readonly Mock<IsendWelcomeEmail> _sendEmailMock;
    private readonly ConfirmEmailCommandHandler _handler;

    public ConfirmEmailCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _backgroundJobMock = new Mock<IBackgroundJobClient>();
        _sendEmailMock = new Mock<IsendWelcomeEmail>();
        _handler = new ConfirmEmailCommandHandler(
            _userRepositoryMock.Object,
            _backgroundJobMock.Object,
            _sendEmailMock.Object
            );
    }

    // Confirm Email success
    [Fact]
    public async Task Handle_whenConfirmationSuccess_ReturnSuccess()
    {
        // Arrenge
        var user = new ApplicationUser
        {
            Id = "1",
            firstName = "Test",
            Email = "Test@test.com",
            EmailConfirmed= false
        };

        var originalCode = "validCode";
        var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(originalCode));

        var DTO = new ConfirmEmailRequestDTO("1",encodedCode);

        _userRepositoryMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.ConfirmEmailAsync(user,originalCode))
            .ReturnsAsync(IdentityResult.Success);

        _sendEmailMock
            .Setup(x => x.sendEmail(It.IsAny<ApplicationUser>()))
            .Returns(Task.CompletedTask);

        _userRepositoryMock
            .Setup(x => x.AddToRoleAsync(user, defaultRoles.Member.Name))
            .ReturnsAsync(IdentityResult.Success);

        _backgroundJobMock
            .Setup(x => x.Create(It.IsAny<Job>(), It.IsAny<IState>()))
            .Returns("jobId");
        // Act 
        var result = await _handler.Handle(new ConfirmEmailCommand(DTO), CancellationToken.None);
        //Assert
        result.IsSuccess.Should().BeTrue();
    }

    // User Not Found

    [Fact]
    public async Task Handle_whenUserNotFound_ReturnFailed()
    {
        // Arrenge

        var originalCode = "validCode";
        var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(originalCode));
        var DTO = new ConfirmEmailRequestDTO("1", encodedCode);
        _userRepositoryMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync((ApplicationUser)null!);


        // Act 
        var result = await _handler.Handle(new ConfirmEmailCommand(DTO), CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.UserNotFound);
    }

    // Already confirmation
    [Fact]
    public async Task Handle_whenAlreadyConfirmed_ReturnFaield()
    {
        //arrenge
        var user = new ApplicationUser
        {
            Id ="1",
            Email = "test@test.com",
            EmailConfirmed = true,
        };

        var DTO = new ConfirmEmailRequestDTO("1", "NotInvalidCode");

        _userRepositoryMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        //act
        var result = await _handler.Handle(new ConfirmEmailCommand(DTO), CancellationToken.None);

        // assert 

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.DuplicateConfirmation);
    }

    // failed to create code
    [Fact]
    public async Task Handle_whenInvalidCode_ReturnFailed()
    {
        //arrenge
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            EmailConfirmed = false,
        };

        _userRepositoryMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);


        var DTO = new ConfirmEmailRequestDTO("1", "InvalidCode!!!!$$##");

        //act
        var result = await _handler.Handle(new ConfirmEmailCommand(DTO), CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.InvalidCode);
    }

    // failed To Confirm
    [Fact]
    public async Task Handle_ConfirmedFailed_ReturnFailed()
    {

        // Arrenge
        var user = new ApplicationUser
        {
            Id = "1",
            firstName = "Test",
            Email = "Test@test.com",
            EmailConfirmed = false
        };

        var originalCode = "validCode";
        var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(originalCode));

        var DTO = new ConfirmEmailRequestDTO("1", encodedCode);

        _userRepositoryMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.ConfirmEmailAsync(user, originalCode))
             .ReturnsAsync(IdentityResult.Failed(new IdentityError
             {
                 Code = "InvalidToken",
                 Description = "Token is invalid."
             }));


        // Act 
        var result = await _handler.Handle(new ConfirmEmailCommand(DTO), CancellationToken.None);
        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("InvalidToken");
        result.Error.Message.Should().Be("Token is invalid.");
        result.Error.StatusCode.Should().Be(400);
    }

    
}
