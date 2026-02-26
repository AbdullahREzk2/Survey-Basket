using Microsoft.AspNetCore.RateLimiting;

namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("ipLimit")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authservice;
    private readonly JwtOptions _jwtoptions;

    public AuthController(IAuthService authService, IOptions<JwtOptions> jwtOptions)
    {
        _authservice = authService;
        _jwtoptions = jwtOptions.Value;
    }

    #region Login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] loginRequestDTO request,CancellationToken cancellationToken)
    {
        var authResult = await _authservice.LoginAsync(request.Email, request.Password,cancellationToken);
        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }
    #endregion

    #region resfresh
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] refreshTokenRequest request,CancellationToken cancellationToken)
    {
        var result = await _authservice.GetRefreshTokenAsync(request.Token, request.RefreshToken,cancellationToken);

        return result.IsSuccess
            ? Ok(result)
            : result.ToProblem();

    }
    #endregion

    #region revoke-refresh-Token
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> revokeRefreshToken([FromBody] refreshTokenRequest request, CancellationToken cancellationToken)
    {
        var isRevoked = await _authservice.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return isRevoked.IsSuccess
            ? Ok()
            :isRevoked.ToProblem();
    }
    #endregion


    #region Register
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request, CancellationToken cancellationToken)
    {
        var result = await _authservice.RegisterAsync(request, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    #endregion


    #region Confirm-Email
    [HttpPost("Confirm-Email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestDTO request)
    {
        var result = await _authservice.ConfirmEmailAsync(request);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    #endregion

    #region Resend-Confirm-Email
    [HttpPost("Resend-Confirm-Email")]
    public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendConfirmationEmailRequest request)
    {
        var result = await _authservice.ResendConfirmationEmailAsync(request);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    #endregion


    #region Forget-Password
    [HttpPost("forget-Password")]
    public async Task<IActionResult> forgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authservice.SendResetPasswordAsync(request);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    #endregion

    #region Reset-Password
    [HttpPost("Reset-Password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authservice.ResetPasswordAsync(request);
        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    #endregion

}
