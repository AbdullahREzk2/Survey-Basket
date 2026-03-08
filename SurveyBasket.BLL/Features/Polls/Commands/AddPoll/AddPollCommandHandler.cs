namespace SurveyBasket.BLL.Features.Polls.Commands.AddPoll;
public class AddPollCommandHandler(IPollRepository pollRepository,IMapper mapper) : IRequestHandler<AddPollCommand, Result<PollResponseDTO>>
{
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<PollResponseDTO>> Handle(AddPollCommand request, CancellationToken cancellationToken)
    {
        var pollEntity = _mapper.Map<Poll>(request.poll);

        var ifExist = await _pollrepository.SearchPollByTitleAsync(pollEntity.Title, cancellationToken);
        if (ifExist)
            return Result.Failure<PollResponseDTO>(PollErrors.PollAlreadyExists);

        var AddedPoll = await _pollrepository.AddPollAsync(pollEntity, cancellationToken);

        return AddedPoll is not null
            ? Result.Success(_mapper.Map<PollResponseDTO>(AddedPoll))
            : Result.Failure<PollResponseDTO>(PollErrors.PollCreationFailed);

    }

}
