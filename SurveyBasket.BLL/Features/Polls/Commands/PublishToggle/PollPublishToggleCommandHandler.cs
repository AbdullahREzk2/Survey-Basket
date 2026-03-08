using SurveyBasket.BLL.Features.Notifications.Command.SendNewPollNotification;

namespace SurveyBasket.BLL.Features.Polls.Commands.PublishToggle;
public class PollPublishToggleCommandHandler(IPollRepository pollRepository,IMediator mediator) : IRequestHandler<PollPublishToggleCommand, Result>
{
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly IMediator _mediator = mediator;

    public async Task<Result> Handle(PollPublishToggleCommand request, CancellationToken cancellationToken)
    {
        var isUpdated = await _pollrepository.publishToggle(request.pollId, cancellationToken);
        if (!isUpdated)
            return Result.Failure(PollErrors.PollPublicationToggleFailed);

        var poll = await _pollrepository.getPollByIdAsync(request.pollId, cancellationToken);
        if (poll.isPublished && poll.startDate == DateOnly.FromDateTime(DateTime.UtcNow))
        {
            BackgroundJob.Enqueue(() =>
                _mediator.Send(new SendNewPollNotificationCommand(request.pollId), CancellationToken.None));
        }

        return Result.Success();
    }
}
