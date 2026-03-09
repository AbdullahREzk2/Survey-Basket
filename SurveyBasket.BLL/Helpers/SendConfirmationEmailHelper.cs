namespace SurveyBasket.BLL.Helpers;
public  class SendConfirmationEmailHelper(IBackgroundJobClient backgroundJob, IOptions<AppURLSetting> appURL)
{
    private readonly IBackgroundJobClient _backgroundjob = backgroundJob;
    private readonly AppURLSetting _appurl = appURL.Value;

    public async Task sendConfirmationEmail(ApplicationUser user, string code)
    {
        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
            {
            { "{name}", user.firstName },
            { "{action_url}", $"{_appurl.BaseUrl}api/auth/Confirm-Email?userId={user.Id}&code={code}" }
            });
        _backgroundjob.Enqueue<IEmailSender>(x =>
            x.SendEmailAsync(user.Email!, "✅ Survey Basket : Email Confirmation", emailBody));
        await Task.CompletedTask;
    }

}
