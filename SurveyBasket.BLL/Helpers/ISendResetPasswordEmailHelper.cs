namespace SurveyBasket.BLL.Helpers;
public interface ISendResetPasswordEmailHelper
{
    Task sendResetPasswordEmail(ApplicationUser user, string code);
}
