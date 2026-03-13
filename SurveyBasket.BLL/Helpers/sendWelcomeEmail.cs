namespace SurveyBasket.BLL.Helpers;
public class sendWelcomeEmail(IBackgroundJobClient backgroundJob) : IsendWelcomeEmail
{
    private readonly IBackgroundJobClient _backgroundjob = backgroundJob;

    public async Task sendEmail(ApplicationUser user)
    {
        var emailBody = EmailBodyBuilder.GenerateEmailBody("Welcome",
            new Dictionary<string, string> { { "{{UserName}}", user.firstName } });
        _backgroundjob.Enqueue<IEmailSender>(x =>
            x.SendEmailAsync(user.Email!, "👋 Survey Basket : Welcome Email", emailBody));
        await Task.CompletedTask;
    }
}
