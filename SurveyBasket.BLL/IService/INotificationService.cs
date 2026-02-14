namespace SurveyBasket.BLL.IService;
public interface INotificationService
{
    Task sendNewNotificationPollAsync(int? pollId = null,CancellationToken cancellationToken=default!);
}
