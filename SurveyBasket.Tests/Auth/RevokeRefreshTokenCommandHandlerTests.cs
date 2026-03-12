using Microsoft.AspNetCore.Identity;
using SurveyBasket.BLL.CurrentUserService;
using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Auth.Command.RevokeRefreshToken;

namespace SurveyBasket.Tests.Auth;
public class RevokeRefreshTokenCommandHandlerTests
{
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly RevokeRefreshTokenCommandHandler _handler;

    public RevokeRefreshTokenCommandHandlerTests()
    {
     _jwtProviderMock = new Mock<IJwtProvider>();
     _userRepoMock = new Mock<IUserRepository>();
     _handler = new RevokeRefreshTokenCommandHandler(
            _jwtProviderMock.Object,
            _userRepoMock.Object
            );
    }
    
    [Fact]
    public async Task Handle_WhenValidRevoke_ReturnSuccess()
    {
        var user = new ApplicationUser
        {
            Id = "1",
            refreshTokens = new List<RefreshToken>
            {
                new RefreshToken
                {
                    Token = "valid-refresh-token",
                    ExpiresOn = DateTime.UtcNow.AddDays(7),
                    RevokedOn = null
                }
            }
        };

        _jwtProviderMock
            .Setup(x=>x.validateToken(It.IsAny<string>()))
            .Returns(user.Id);

        _userRepoMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userRepoMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _handler.Handle(request: new RevokeRefreshTokenCommand("Token", "valid-refresh-token"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

    }

    [Fact]
    public async Task Handle_WhenInvalidToken_ReturnFailure()
    {
        
        _jwtProviderMock
            .Setup(x => x.validateToken(It.IsAny<string>()))
            .Returns((string)null!);

        var result = await _handler.Handle(new RevokeRefreshTokenCommand("Token", "valid-refresh-token"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.InvalidToken);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ReturnFailure()
    {

        _jwtProviderMock
            .Setup(x => x.validateToken(It.IsAny<string>()))
            .Returns("1");


        _userRepoMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser)null!);

        var result = await _handler.Handle(request: new RevokeRefreshTokenCommand("Token", "valid-refresh-token"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.UserNotFound);
    }

    [Fact]
    public async Task Handle_WhenRefreshTokenNotFound_ReturnFailure()
    {
        var user = new ApplicationUser
        {
            Id = "1",
            refreshTokens = new List<RefreshToken>
            {
                new RefreshToken
                {
                    Token = null!,
                    ExpiresOn = DateTime.UtcNow.AddDays(7),
                    RevokedOn = null
                }
            }
        };

        _jwtProviderMock
            .Setup(x => x.validateToken(It.IsAny<string>()))
            .Returns(user.Id);

        _userRepoMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        var result = await _handler.Handle(request: new RevokeRefreshTokenCommand("Token", "valid-refresh-token"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.RefreshTokenNotFound);
    }

    [Fact]
    public async Task Handle_Always_CallsUpdateOnce()
    {
        var user = new ApplicationUser
        {
            Id = "1",
            refreshTokens = new List<RefreshToken>
            {
                new RefreshToken
                {
                    Token = "valid-refresh-token",
                    ExpiresOn = DateTime.UtcNow.AddDays(7),
                    RevokedOn = null
                }
            }
        };

        _jwtProviderMock
            .Setup(x => x.validateToken(It.IsAny<string>()))
            .Returns(user.Id);

        _userRepoMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userRepoMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _handler.Handle(request: new RevokeRefreshTokenCommand("Token", "valid-refresh-token"), CancellationToken.None);

        _userRepoMock.Verify(

            x => x.UpdateAsync(user),
            Times.Once
            );
    }

}
