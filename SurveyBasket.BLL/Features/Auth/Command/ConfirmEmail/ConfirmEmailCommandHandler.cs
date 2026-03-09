namespace SurveyBasket.BLL.Features.Auth.Command.ConfirmEmail;
public class ConfirmEmailCommandHandler(IUserRepository userRepository,IBackgroundJobClient backgroundJob) : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly IUserRepository _userrepository = userRepository;
    private readonly IBackgroundJobClient _backgroundjob = backgroundJob;

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {

        var user = await _userrepository.FindByIdAsync(request.emailRequest.UserId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicateConfirmation);

        string code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.emailRequest.Code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await _userrepository.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            await sendWelcomeEmail(user);
            await _userrepository.AddToRoleAsync(user, defaultRoles.Member.Name);
            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    private async Task sendWelcomeEmail(ApplicationUser user)
    {
        var emailBody = EmailBodyBuilder.GenerateEmailBody("Welcome",
            new Dictionary<string, string> { { "{{UserName}}", user.firstName } });
        _backgroundjob.Enqueue<IEmailSender>(x =>
            x.SendEmailAsync(user.Email!, "👋 Survey Basket : Welcome Email", emailBody));
        await Task.CompletedTask;
    }

}
