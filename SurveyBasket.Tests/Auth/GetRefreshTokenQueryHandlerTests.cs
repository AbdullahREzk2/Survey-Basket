using Microsoft.AspNetCore.Identity;
using SurveyBasket.BLL.CurrentUserService;
using SurveyBasket.BLL.Errors;
using SurveyBasket.BLL.Features.Auth.Query.GetRefreshToken;

namespace SurveyBasket.Tests.Auth;
public class GetRefreshTokenQueryHandlerTests
{
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly GetRefreshTokenQueryHandler _handler;

    public GetRefreshTokenQueryHandlerTests()
    {
     _jwtProviderMock= new Mock<IJwtProvider>();
      _userRepoMock= new Mock<IUserRepository>();
        _handler = new GetRefreshTokenQueryHandler(
            _jwtProviderMock.Object,
             _userRepoMock.Object
            );
    }

    [Fact]
    public async Task Handle_WhenValidRefreshToken_ReturnSuccess()
    {
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            isDisabled = false,
            LockoutEnd = null,
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
            .Setup(x=>x.GetRolesAsync(user))
            .ReturnsAsync(new List<string>());

        _userRepoMock
            .Setup(x => x.GetUserPermissionsAsync(It.IsAny<IList<string>>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string>());

        _jwtProviderMock
            .Setup(x => x.GenerateToken(user, It.IsAny<IList<string>>(), It.IsAny<IList<string>>()))
            .Returns(("newToken", 30));

        _userRepoMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _handler.Handle(new GetRefreshTokenQuery("Token", "valid-refresh-token"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Token.Should().Be("newToken");
    }
    [Fact]
    public async Task Handle_WhenInvalidToken_ReturnFailure()
    {
        _jwtProviderMock
            .Setup(x => x.validateToken(It.IsAny<string>()))
            .Returns((string)null!);

        var result = await _handler.Handle(new GetRefreshTokenQuery("Token", "valid-refresh-token"), CancellationToken.None);

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

        var result = await _handler.Handle(new GetRefreshTokenQuery("Token", "valid-refresh-token"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.UserNotFound);
    }
    [Fact]
    public async Task Handle_WhenUserDisabled_ReturnFailure()
    {
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            isDisabled = true,
            LockoutEnd = null,
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

        var result = await _handler.Handle(new GetRefreshTokenQuery("Token", "valid-refresh-token"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.DisabledUser);
    }
    [Fact]
    public async Task Handle_WhenUserLockedOut_ReturnFailure()
    {
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            isDisabled = false,
            LockoutEnd = DateTime.UtcNow.AddDays(2),
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

        var result = await _handler.Handle(new GetRefreshTokenQuery("Token", "valid-refresh-token"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.UserLockedOut);

    }
    [Fact]
    public async Task Handle_WhenRefreshTokenNotFound_ReturnFailure()
    {
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "test@test.com",
            isDisabled = false,
            LockoutEnd = null,
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

        var result = await _handler.Handle(new GetRefreshTokenQuery("Token", "valid-refresh-token"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.RefreshTokenNotFound);

    }


}
