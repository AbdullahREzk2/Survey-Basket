namespace SurveyBasket.BLL.Service;
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _usermanager;
    private readonly SignInManager<ApplicationUser> _signinmanager;
    private readonly IJwtProvider _jwtprovider;
    private readonly ILogger<AuthService> _logger;
    private readonly IEmailSender _emailsender;
    private readonly int _refreshTokenValidityInDays = 14;

    public AuthService
        (
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager ,
        IJwtProvider jwtProvider,
        ILogger<AuthService> logger,
        IEmailSender emailSender
        )
    {
        _usermanager = userManager;
        _signinmanager = signInManager;
        _jwtprovider = jwtProvider;
        _logger = logger;
        _emailsender = emailSender;
    }

    public async Task<Result<loginResponseDTO>> LoginAsync(string Email, string Password, CancellationToken cancellationToken)
    {
        // check user 
        if (await _usermanager.FindByEmailAsync(Email) is not { } user)
            return Result.Failure<loginResponseDTO>(UserErrors.InvalidCredentials);

        // check password
        var result = await _signinmanager.PasswordSignInAsync(user, Password, false, false);

        if(result.Succeeded)
        {
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

        return Result.Failure<loginResponseDTO>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials);
        
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
    public async Task<Result> RegisterAsync(RegisterRequestDTO requestDTO, CancellationToken cancellationToken)
    {
        var EmailExists = await _usermanager.Users.AnyAsync(x=>x.Email == requestDTO.Email);
        if (EmailExists)
            return Result.Failure(UserErrors.DuplicatedEmail);

        var user = requestDTO.Adapt<ApplicationUser>();

        var result = await _usermanager.CreateAsync(user, requestDTO.Password);

        if (result.Succeeded)
        {
            var code = await _usermanager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirmation code : {code}", code);

            await sendConfirmationEmail(user, code);

            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequestDTO request)
    {
        if (await _usermanager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicateConfirmation);

        var code = request.Code;

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await _usermanager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }
    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        if (await _usermanager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicateConfirmation);

        var code = await _usermanager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Confirmation code :{code}", code);

        await sendConfirmationEmail(user, code);

        return Result.Success();
    }



    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
    private async Task sendConfirmationEmail(ApplicationUser user , string code)
    {
        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
             {
                    {"{name}",user.firstName }
             }
            );
        await _emailsender.SendEmailAsync(user.Email!, " ✅ Survey Basket : Email Confirmation", emailBody);
    }

}




