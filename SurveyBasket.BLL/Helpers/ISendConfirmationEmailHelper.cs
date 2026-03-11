namespace SurveyBasket.BLL.Helpers;
public interface ISendConfirmationEmailHelper
{
    Task sendConfirmationEmail(ApplicationUser user, string code);
}
