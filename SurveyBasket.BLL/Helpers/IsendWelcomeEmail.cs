namespace SurveyBasket.BLL.Helpers;
public interface IsendWelcomeEmail
{
    Task sendEmail(ApplicationUser user);
}
