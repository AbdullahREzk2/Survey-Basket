namespace SurveyBasket.BLL.Service;
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _usermanager;
    private readonly IJwtProvider _jwtprovider;

    private readonly int _refreshTokenValidityInDays = 14;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
    {
        _usermanager = userManager;
        _jwtprovider = jwtProvider;
    }

    public async Task<Result<loginResponseDTO>> LoginAsync(string Email, string Password, CancellationToken cancellationToken)
    {
        // check user 
        var user = await _usermanager.FindByEmailAsync(Email);

        if (user is null)
            return Result.Failure<loginResponseDTO>(UserErrors.InvalidCredentials);

        // check password
        bool isPasswordValid = await _usermanager.CheckPasswordAsync(user, Password);

        if (!isPasswordValid)
            return Result.Failure<loginResponseDTO>(UserErrors.InvalidCredentials);

        // generate token
        var (token, expireIn) = _jwtprovider.GenerateToken(user);

        // generate refresh token
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenValidityInDays);

        user.refreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration,
        });
        await _usermanager.UpdateAsync(user);


        // return response
        var response = new loginResponseDTO(user.Id, user.Email!, user.UserName!, user.firstName, user.lastName, token, expireIn, refreshToken, refreshTokenExpiration);
        return Result.Success(response);
    }
    public async Task<Result<loginResponseDTO>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        // get the User Id from the token
        var userId = _jwtprovider.validateToken(token);
        if (userId is null)
            return Result.Failure<loginResponseDTO>(UserErrors.InvalidToken);

        // get the user from the database
        var user = await _usermanager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<loginResponseDTO>(UserErrors.UserNotFound);

        // get the refresh token from the user
        var userRefreshToken = user.refreshTokens.SingleOrDefault(rt => rt.Token == refreshToken && rt.IsActive);
        if (userRefreshToken is null)
            return Result.Failure<loginResponseDTO>(UserErrors.RefreshTokenNotFound);

        // revoke the old refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        // generate new jwt token & refresh token
        var (newToken, expireIn) = _jwtprovider.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenValidityInDays);

        user.refreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration,
        });
        await _usermanager.UpdateAsync(user);

        // return response
        var response = new loginResponseDTO(user.Id, user.Email!, user.UserName!, user.firstName, user.lastName, newToken, expireIn, newRefreshToken, refreshTokenExpiration);

        return Result.Success(response);
    }
    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        // get the User Id from the token
        var userId = _jwtprovider.validateToken(token);
        if (userId is null)
            return Result.Failure(UserErrors.InvalidToken);

        // get the user from the database
        var user = await _usermanager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        // get the refresh token from the user
        var userRefreshToken = user.refreshTokens.SingleOrDefault(rt => rt.Token == refreshToken && rt.IsActive);
        if (userRefreshToken is null)
            return Result.Failure(UserErrors.RefreshTokenNotFound);

        // revoke the old refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _usermanager.UpdateAsync(user);

        return Result.Success();

    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

}



