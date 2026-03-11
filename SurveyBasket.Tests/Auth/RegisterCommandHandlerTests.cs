using Castle.Core.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SurveyBasket.BLL.Contracts.Authantication;
using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Auth.Command.Register;
using SurveyBasket.BLL.Helpers;

namespace SurveyBasket.Tests.Auth;
public class RegisterCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<RegisterCommandHandler>> _userLoggerMock;
    private readonly Mock<ISendConfirmationEmailHelper> _confirmationEmailHelperMock;
    private readonly RegisterCommandHandler _handler;
    public RegisterCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userLoggerMock = new Mock<ILogger<RegisterCommandHandler>>();
        _confirmationEmailHelperMock = new Mock<ISendConfirmationEmailHelper>();
        _handler = new RegisterCommandHandler(
            _userRepositoryMock.Object,
            _userLoggerMock.Object,
            _confirmationEmailHelperMock.Object
            );
    }

    // when registerSucess -->
    [Fact]
    public async Task Handle_whenRegisterSuccess_ReturnSucess()
    {
        //arrenge
        var DTO = new RegisterRequestDTO(Email: "test.com", "anyPass", "ahmed", "khaled");

        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x=>x.CreateAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _userRepositoryMock
            .Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync("Email-Token");

        _confirmationEmailHelperMock
            .Setup(x=>x.sendConfirmationEmail(It.IsAny<ApplicationUser>(),It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // act
        var result = await _handler.Handle(new RegisterCommand(DTO), CancellationToken.None);
        // assert
        result.IsSuccess.Should().BeTrue();
    }

    // when email already exist 
    [Fact]
    public async Task Handle_whenUserExist_ReturnFailed()
    {
        // arrenge
        var DTO = new RegisterRequestDTO(Email: "test.com", "anyPass", "ahmed", "khaled");

        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // act 
        var result = await _handler.Handle(new RegisterCommand(DTO), CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.DuplicatedEmail);
    }

    //when creation failed -->
    [Fact]
    public async Task Handle_whenCreationFaield_ReturnFailed()
    {
        //arrange
        var DTO = new RegisterRequestDTO(Email: "test.com", "anyPass", "ahmed", "khaled");
        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Code = "Creation Failed",
                Description = "Error",
            }));

        //act
        var result = await _handler.Handle(new RegisterCommand(DTO), CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.Error.StatusCode.Should().Be(400);
        result.Error.Code.Should().Be("Creation Failed");
    }

    // repo call once
    [Fact]
    public async Task Handle_always_HitRepoOnce()
    {
        //arrenge
        var DTO = new RegisterRequestDTO(Email: "test.com", "anyPass", "ahmed", "khaled");

        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _userRepositoryMock
            .Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync("Email-Token");

        _confirmationEmailHelperMock
            .Setup(x => x.sendConfirmationEmail(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // act
        var result = await _handler.Handle(new RegisterCommand(DTO), CancellationToken.None);
        // assert
        _userRepositoryMock.Verify(
            x=>x.CreateAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>()),
            Times.Once
            );
    }

}
