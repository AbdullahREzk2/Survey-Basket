namespace SurveyBasket.BLL.Helpers;
public class SendResetPasswordEmailHelper(IBackgroundJobClient backgroundJob, IOptions<AppURLSetting> appURL)
{
    private readonly IBackgroundJobClient _backgroundjob = backgroundJob;
    private readonly AppURLSetting _appurl = appURL.Value;

    public async Task sendResetPasswordEmail(ApplicationUser user, string code)
    {
        var emailBody = EmailBodyBuilder.GenerateEmailBody("ResetPassword",
            new Dictionary<string, string>
            {
            { "{{UserName}}", user.firstName },
            { "{{ResetLink}}", $"{_appurl.BaseUrl}api/auth/Reset-Password?email={user.Email}&code={code}" }
            });
        _backgroundjob.Enqueue<IEmailSender>(x =>
            x.SendEmailAsync(user.Email!, "🔑 Survey Basket : Reset Password Email", emailBody));
        await Task.CompletedTask;
    }

}
