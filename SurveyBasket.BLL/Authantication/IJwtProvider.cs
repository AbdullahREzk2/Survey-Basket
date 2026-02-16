namespace SurveyBasket.BLL.CurrentUserService;
public interface IJwtProvider
{
    (string token , int expireIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
    string? validateToken(string token);
}
