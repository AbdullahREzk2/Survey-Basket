namespace SurveyBasket.DAL.Entities;
public class ApplicationUser:IdentityUser
{
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; }  = string.Empty;
    public bool isDisabled { get; set; }
    public List<RefreshToken> refreshTokens { get; set; } = [];
}
