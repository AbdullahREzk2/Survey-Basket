namespace SurveyBasket.BLL.Features.Polls.Commands.PublishToggle;
public class PollPublishToggleCommandHandler(IPollRepository pollRepository) : IRequestHandler<PollPublishToggleCommand, Result>
{
    private readonly IPollRepository _pollrepository = pollRepository;

    public async Task<Result> Handle(PollPublishToggleCommand request, CancellationToken cancellationToken)
    {
        var isUpdated = await _pollrepository.publishToggle(request.pollId, cancellationToken);
        if (!isUpdated)
            return Result.Failure(PollErrors.PollPublicationToggleFailed);
        var poll = await _pollrepository.getPollByIdAsync(request.pollId, cancellationToken);
        if (poll.isPublished && poll.startDate == DateOnly.FromDateTime(DateTime.UtcNow))
        {
            BackgroundJob.Enqueue<INotificationService>(
                service => service.sendNewNotificationPollAsync(request.pollId, CancellationToken.None));
        }
        return Result.Success();
    }
}
