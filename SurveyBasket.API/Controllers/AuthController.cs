namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
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
    public async Task<IActionResult> revokeRefreshTokenAsync([FromBody] refreshTokenRequest request, CancellationToken cancellationToken)
    {
        var isRevoked = await _authservice.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return isRevoked.IsSuccess
            ? Ok()
            :isRevoked.ToProblem();
    }
    #endregion



}
