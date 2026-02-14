using SurveyBasket.BLL.Settings;

namespace SurveyBasket.BLL.Service;
public class NotificationService(
    IPollRepository pollRepository,
    UserManager<ApplicationUser> userManager,
    IEmailSender emailSender,
    IOptions<AppURLSetting> appUrlSettings
    ) : INotificationService
{
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly UserManager<ApplicationUser> _usermanager = userManager;
    private readonly IEmailSender _emailsender = emailSender;
    private readonly string _baseUrl = appUrlSettings.Value.BaseUrl;

    public async Task sendNewNotificationPollAsync(int? pollId = null,CancellationToken cancellationToken = default)
    {
        IEnumerable<PollResponseDTO> pollsItem;

        if (pollId.HasValue)
        {
            var pollModel = await _pollrepository.getTodayPoll(pollId, cancellationToken);
            pollsItem = pollModel.Adapt<IEnumerable<PollResponseDTO>>();
        }
        else
        {
            var pollsModels = await _pollrepository.getTodayPolls(cancellationToken);
            pollsItem = pollsModels.Adapt<IEnumerable<PollResponseDTO>>();
        }

        if (!pollsItem.Any())
            return;

        var pollItemsHtml = GeneratePollItemsHtml(pollsItem);

        var users = await _usermanager.Users.ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            var placeHolders = new Dictionary<string, string>
        {
            {"{UserName}", user.firstName},
            {"{PollItems}", pollItemsHtml}
        };

            var body = EmailBodyBuilder.GenerateEmailBody("DailyPollNotification", placeHolders);

            await _emailsender.SendEmailAsync(user.Email!,"🎤 New Polls Available Today",body);
        }
    }

    private string GeneratePollItemsHtml(IEnumerable<PollResponseDTO> polls)
    {
        var builder = new StringBuilder();

        foreach (var poll in polls)
        {
            builder.Append($@"
        <table width='100%' cellpadding='0' cellspacing='0' border='0' style='margin-bottom:20px; border:1px solid #e5e7eb; border-radius:6px;'>
            <tr>
                <td style='padding:15px;'>
                    <h3 style='margin:0 0 10px 0; color:#111827;'>
                        {poll.Title}
                    </h3>
                    <p style='margin:0 0 15px 0; color:#6b7280; font-size:14px;'>
                        {poll.Description}
                    </p>

                        <a href='{_baseUrl}/polls/{poll.PollId}'
                       style='display:inline-block; padding:10px 18px; background-color:#2563eb; color:#ffffff; text-decoration:none; border-radius:4px; font-size:14px;'>
                        Participate Now
                    </a>
                </td>
            </tr>
        </table>");
        }

        return builder.ToString();
    }

}
