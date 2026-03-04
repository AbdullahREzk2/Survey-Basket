namespace SurveyBasket.BLL.Service;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userrepository;
    private readonly SignInManager<ApplicationUser> _signinmanager;
    private readonly IJwtProvider _jwtprovider;
    private readonly ILogger<AuthService> _logger;
    private readonly IEmailSender _emailsender;
    private readonly IBackgroundJobClient _backgroundjob;
    private readonly IHttpContextAccessor _httpcontextaccessor;
    private readonly int _refreshTokenValidityInDays = 14;

    public AuthService(
        IUserRepository userRepository,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider,
        ILogger<AuthService> logger,
        IEmailSender emailSender,
        IBackgroundJobClient backgroundJob,
        IHttpContextAccessor httpContextAccessor)
    {
        _userrepository = userRepository;
        _signinmanager = signInManager;
        _jwtprovider = jwtProvider;
        _logger = logger;
        _emailsender = emailSender;
        _backgroundjob = backgroundJob;
        _httpcontextaccessor = httpContextAccessor;
    }

    public async Task<Result<loginResponseDTO>> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userrepository.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure<loginResponseDTO>(UserErrors.InvalidCredentials);

        if (user.isDisabled)
            return Result.Failure<loginResponseDTO>(UserErrors.DisabledUser);

        var result = await _signinmanager.PasswordSignInAsync(user, password, false, true);

        if (result.Succeeded)
        {
            var userRoles = await _userrepository.GetRolesAsync(user);
            var userPermissions = await _userrepository.GetUserPermissionsAsync(userRoles, cancellationToken);

            var (token, expireIn) = _jwtprovider.GenerateToken(user, userRoles, userPermissions);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenValidityInDays);

            user.refreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration,
            });
            await _userrepository.UpdateAsync(user);

            var response = new loginResponseDTO(user.Id, user.Email!, user.UserName!, user.firstName, user.lastName, token, expireIn, refreshToken, refreshTokenExpiration);
            return Result.Success(response);
        }

        var error = result.IsNotAllowed ? UserErrors.EmailNotConfirmed
            : result.IsLockedOut ? UserErrors.UserLockedOut
            : UserErrors.InvalidCredentials;

        return Result.Failure<loginResponseDTO>(error);
    }

    public async Task<Result<loginResponseDTO>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        var userId = _jwtprovider.validateToken(token);
        if (userId is null)
            return Result.Failure<loginResponseDTO>(UserErrors.InvalidToken);

        var user = await _userrepository.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<loginResponseDTO>(UserErrors.UserNotFound);

        if (user.isDisabled)
            return Result.Failure<loginResponseDTO>(UserErrors.DisabledUser);

        if (user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<loginResponseDTO>(UserErrors.UserLockedOut);

        var userRefreshToken = user.refreshTokens.SingleOrDefault(rt => rt.Token == refreshToken && rt.IsActive);
        if (userRefreshToken is null)
            return Result.Failure<loginResponseDTO>(UserErrors.RefreshTokenNotFound);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var userRoles = await _userrepository.GetRolesAsync(user);
        var userPermissions = await _userrepository.GetUserPermissionsAsync(userRoles, cancellationToken);

        var (newToken, expireIn) = _jwtprovider.GenerateToken(user, userRoles, userPermissions);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenValidityInDays);

        user.refreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration,
        });
        await _userrepository.UpdateAsync(user);

        var response = new loginResponseDTO(user.Id, user.Email!, user.UserName!, user.firstName, user.lastName, newToken, expireIn, newRefreshToken, refreshTokenExpiration);
        return Result.Success(response);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        var userId = _jwtprovider.validateToken(token);
        if (userId is null)
            return Result.Failure(UserErrors.InvalidToken);

        var user = await _userrepository.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var userRefreshToken = user.refreshTokens.SingleOrDefault(rt => rt.Token == refreshToken && rt.IsActive);
        if (userRefreshToken is null)
            return Result.Failure(UserErrors.RefreshTokenNotFound);

        userRefreshToken.RevokedOn = DateTime.UtcNow;
        await _userrepository.UpdateAsync(user);

        return Result.Success();
    }

    public async Task<Result> RegisterAsync(RegisterRequestDTO requestDTO, CancellationToken cancellationToken)
    {
        if (await _userrepository.EmailExistsAsync(requestDTO.Email))
            return Result.Failure(UserErrors.DuplicatedEmail);

        var user = requestDTO.Adapt<ApplicationUser>();
        var result = await _userrepository.CreateAsync(user, requestDTO.Password);

        if (result.Succeeded)
        {
            var code = await _userrepository.GenerateEmailConfirmationTokenAsync(user);
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
        var user = await _userrepository.FindByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicateConfirmation);

        string code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await _userrepository.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            await sendWelcomeEmail(user);
            await _userrepository.AddToRoleAsync(user, defaultRoles.Member.Name);
            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        var user = await _userrepository.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicateConfirmation);

        var code = await _userrepository.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("Confirmation code: {code}", code);
        await sendConfirmationEmail(user, code);

        return Result.Success();
    }

    public async Task<Result> SendResetPasswordAsync(ForgetPasswordRequest request)
    {
        var user = await _userrepository.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailNotConfirmed);

        var code = await _userrepository.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("Reset Password code: {code}", code);
        await sendResetPasswordEmail(user, code);

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userrepository.FindByEmailAsync(request.Email);
        if (user is null || !user.EmailConfirmed)
            return Result.Failure(UserErrors.UserNotFound);

        IdentityResult result;
        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            result = await _userrepository.ResetPasswordAsync(user, code, request.NewPassword);
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
    }


    private static string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    private async Task sendConfirmationEmail(ApplicationUser user, string code)
    {
        var origin = _httpcontextaccessor.HttpContext?.Request.Headers.Origin;
        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
            {
                { "{name}", user.firstName },
                { "{action_url}", $"{origin}/auth/confirmEmail?email={user.Email}&code={code}" }
            });
        _backgroundjob.Enqueue<IEmailSender>(x =>
            x.SendEmailAsync(user.Email!, "✅ Survey Basket : Email Confirmation", emailBody));
        await Task.CompletedTask;
    }

    private async Task sendResetPasswordEmail(ApplicationUser user, string code)
    {
        var origin = _httpcontextaccessor.HttpContext?.Request.Headers.Origin;
        var emailBody = EmailBodyBuilder.GenerateEmailBody("ResetPassword",
            new Dictionary<string, string>
            {
                { "{{UserName}}", user.firstName },
                { "{{ResetLink}}", $"{origin}/auth/forgetPassword?email={user.Email}&code={code}" }
            });
        _backgroundjob.Enqueue<IEmailSender>(x =>
            x.SendEmailAsync(user.Email!, "🔑 Survey Basket : Reset Password Email", emailBody));
        await Task.CompletedTask;
    }

    private async Task sendWelcomeEmail(ApplicationUser user)
    {
        var emailBody = EmailBodyBuilder.GenerateEmailBody("Welcome",
            new Dictionary<string, string> { { "{{UserName}}", user.firstName } });
        _backgroundjob.Enqueue<IEmailSender>(x =>
            x.SendEmailAsync(user.Email!, "👋 Survey Basket : Welcome Email", emailBody));
        await Task.CompletedTask;
    }


}