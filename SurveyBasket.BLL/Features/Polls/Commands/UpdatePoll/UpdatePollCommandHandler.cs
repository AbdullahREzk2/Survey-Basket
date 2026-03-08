namespace SurveyBasket.BLL.Features.Polls.Commands.UpdatePoll;
public class UpdatePollCommandHandler(IPollRepository pollRepository,IMapper mapper) : IRequestHandler<UpdatePollCommand, Result<PollResponseDTO>>
{
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<PollResponseDTO>> Handle(UpdatePollCommand request, CancellationToken cancellationToken)
    {
        var pollEntity = _mapper.Map<Poll>(request.poll);

        var ifExist = await _pollrepository.getPollByIdAsync(request.pollId, cancellationToken);

        if (ifExist is null)
            return Result.Failure<PollResponseDTO>(PollErrors.PollNotFound);

        var updatedPoll = await _pollrepository.UpdatePollAsync(request.pollId, pollEntity, cancellationToken);

        return updatedPoll is not null
            ? Result.Success(_mapper.Map<PollResponseDTO>(updatedPoll))
            : Result.Failure<PollResponseDTO>(PollErrors.PollUPdateFailed);
    }
}
