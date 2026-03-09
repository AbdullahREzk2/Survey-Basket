namespace SurveyBasket.BLL.Helpers;
public static class GenerateRefreshTokenHelper
{
    public static string GenerateRefreshToken()
    => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

}
