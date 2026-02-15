namespace SurveyBasket.BLL.IService;
public interface IAuthService
{
    Task<Result<loginResponseDTO>> LoginAsync(string Email, string Password,CancellationToken cancellationToken);
    Task<Result<loginResponseDTO>> GetRefreshTokenAsync(string token, string refreshToken,CancellationToken cancellationToken);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken,CancellationToken cancellationToken);

   Task<Result> RegisterAsync(RegisterRequestDTO requestDTO,CancellationToken cancellationToken);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequestDTO request);
    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request);
    Task<Result> SendResetPasswordAsync(ForgetPasswordRequest request);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
}
