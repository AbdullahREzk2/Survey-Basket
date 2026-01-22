namespace SurveyBasket.BLL.IService;
public interface IAuthService
{
    Task<loginResponseDTO?> LoginAsync(string Email, string Password,CancellationToken cancellationToken);
    Task<loginResponseDTO?> GetRefreshTokenAsync(string token, string refreshToken,CancellationToken cancellationToken);
    Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken,CancellationToken cancellationToken);

}
