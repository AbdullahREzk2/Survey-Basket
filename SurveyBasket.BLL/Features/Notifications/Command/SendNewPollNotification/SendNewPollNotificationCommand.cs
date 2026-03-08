namespace SurveyBasket.BLL.Features.Notifications.Command.SendNewPollNotification;
public record SendNewPollNotificationCommand(int? PollId = null) : IRequest<Unit>;
