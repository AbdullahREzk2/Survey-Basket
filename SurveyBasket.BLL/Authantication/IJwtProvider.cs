namespace SurveyBasket.BLL.CurrentUserService;
public interface IJwtProvider
{
    (string token , int expireIn) GenerateToken(ApplicationUser user);
    string? validateToken(string token);
}
