namespace SurveyBasket.BLL.Features.Polls.Commands.DeletePoll;
public class DeletePollCommandHandler(IPollRepository pollRepository) : IRequestHandler<DeletePollCommand, Result>
{
    private readonly IPollRepository _pollrepository = pollRepository;

    public async Task<Result> Handle(DeletePollCommand request, CancellationToken cancellationToken)
    {
        var pollExist = await _pollrepository.getPollByIdAsync(request.pollId, cancellationToken);
        if (pollExist is null)
            return Result.Failure(PollErrors.PollNotFound);

        var response = await _pollrepository.DeletePollAsync(request.pollId, cancellationToken);
        if (response)
            return Result.Success();

        return Result.Failure(PollErrors.PollDeletionFailed);
    }
}
