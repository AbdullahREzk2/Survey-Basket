using System.Text;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.BLL.Contracts.Authantication;
using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Auth.Command.ResetPassword;

namespace SurveyBasket.Tests.Auth;
public class ResetPasswordCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly ResetPasswordCommandHandler _handler;
    public ResetPasswordCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new ResetPasswordCommandHandler(
            _userRepositoryMock.Object
            );
    }
    [Fact]
    public async Task Handle_WhenValidReset_ReturnSuccess()
    {
        //arrenge 
        var code = "TestCode";
        var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var DTO = new ResetPasswordRequest("test@test.com", encodedCode, "AnyPass");
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            EmailConfirmed = true,

        };
        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.ResetPasswordAsync(user, code, "AnyPass"))
            .ReturnsAsync(IdentityResult.Success);
        //act
        var result = await _handler.Handle(new ResetPasswordCommand(DTO), CancellationToken.None);

        //assert

        result.IsSuccess.Should().BeTrue();
            
    } 
    [Fact]
    public async Task Handle_WhenUserNotFound_ReturnFailure()
    {
        _userRepositoryMock
             .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
             .ReturnsAsync((ApplicationUser)null!);
        var DTO = new ResetPasswordRequest("test@test.com", "AnyCode", "AnyPass");

        var result = await _handler.Handle(new ResetPasswordCommand(DTO), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.UserNotFound);
    }
    [Fact]
    public async Task Handle_WhenEmailNotConfirmed_ReturnFailure()
    {
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            EmailConfirmed = false,
        };

        _userRepositoryMock
             .Setup(x => x.FindByEmailAsync("test@test.com"))
             .ReturnsAsync(user);

        var DTO = new ResetPasswordRequest("test@test.com", "AnyCode", "AnyPass");

        var result = await _handler.Handle(new ResetPasswordCommand(DTO), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.UserNotFound);
    }
    [Fact]
    public async Task Handle_WhenInvalidCode_ReturnFailure()
    {
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            EmailConfirmed = true
        };

        _userRepositoryMock
             .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
             .ReturnsAsync(user);


        var DTO = new ResetPasswordRequest("test@test.com", "AnyCodee@@", "AnyPass");

        var result = await _handler.Handle(new ResetPasswordCommand(DTO), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.InvalidCode);
    }
    [Fact]
    public async Task Handle_WhenResetFailed_ReturnFailure()
    {
        //arrenge 
        var code = "TestCode";
        var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var DTO = new ResetPasswordRequest("test@test.com", encodedCode, "AnyPass");
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            EmailConfirmed = true,

        };
        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.ResetPasswordAsync(user, code, "AnyPass"))
            .ReturnsAsync(IdentityResult.Failed (new IdentityError
            {
                Code = "Failed",
                Description = "failed",
            }));
        //act
        var result = await _handler.Handle(new ResetPasswordCommand(DTO), CancellationToken.None);

        //assert

        result.IsSuccess.Should().BeFalse();
        result.Error.StatusCode.Should().Be(401);
        result.Error.Code.Should().Be("Failed");
    }


}
