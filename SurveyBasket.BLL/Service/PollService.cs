namespace SurveyBasket.BLL.Service;
public class PollService : IPollService
{
    private readonly IPollRepository _pollrepository;
    private readonly IMapper _mapper;

    public PollService(IPollRepository pollRepository,IMapper mapper)
    {
        _pollrepository = pollRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<PollResponseDTO>>> getAllPollsAsync(CancellationToken cancellationToken)
    {
        var polls = await _pollrepository.getAllPollsAsync(cancellationToken);

        var response = _mapper.Map<IEnumerable<PollResponseDTO>>(polls);

        return response is not null
            ?Result.Success(response)
            :Result.Failure<IEnumerable<PollResponseDTO>>(PollErrors.PollNotFound);
    }
    public async Task<Result<IEnumerable<PollResponseDTO>>> getAvailblePollsAsync(CancellationToken cancellationToken)
    {
        var polls = await _pollrepository.getAvailblePollsAsync(cancellationToken);

        var response = polls.Adapt<IEnumerable<PollResponseDTO>>();

        return response is not null
            ? Result.Success(response)
            : Result.Failure<IEnumerable<PollResponseDTO>>(PollErrors.PollNotFound);
    }
    public async Task<Result<PollResponseDTO>> getPollByIdAsync(int pollId, CancellationToken cancellationToken)
    {
        var poll = await _pollrepository.getPollByIdAsync(pollId,cancellationToken);

        var response = _mapper.Map<PollResponseDTO>(poll);

        return response is not null
            ?Result.Success(response)
            :Result.Failure<PollResponseDTO>(PollErrors.PollNotFound);
    }
    public async Task<Result<PollResponseDTO>> AddPollAsync(PollRequestDTO poll, CancellationToken cancellationToken)
    {
        var pollEntity = _mapper.Map<Poll>(poll);  

        var ifExist = await _pollrepository.SearchPollByTitleAsync(pollEntity.Title, cancellationToken);
        if(ifExist)
            return Result.Failure<PollResponseDTO>(PollErrors.PollAlreadyExists);

        var AddedPoll = await _pollrepository.AddPollAsync(pollEntity, cancellationToken);

        return AddedPoll is not null
            ? Result.Success(_mapper.Map<PollResponseDTO>(AddedPoll))
            : Result.Failure<PollResponseDTO>(PollErrors.PollCreationFailed);

    }
    public async Task<Result<PollResponseDTO>> UpdatePollAsync(int pollId, PollRequestDTO poll,CancellationToken cancellationToken)
    {
        var pollEntity = _mapper.Map<Poll>(poll);

        var ifExist = await _pollrepository.getPollByIdAsync(pollId, cancellationToken);
        if (ifExist is null)
            return Result.Failure<PollResponseDTO>(PollErrors.PollNotFound);

        var updatedPoll = await _pollrepository.UpdatePollAsync(pollId, pollEntity,cancellationToken);

        return updatedPoll is not null
            ? Result.Success(_mapper.Map<PollResponseDTO>(updatedPoll))
            : Result.Failure<PollResponseDTO>(PollErrors.PollUPdateFailed);
    }
    public async Task<Result> DeletePollAsync(int pollId,CancellationToken cancellationToken)
    {
        var pollExist = await _pollrepository.getPollByIdAsync(pollId, cancellationToken);
        if (pollExist is null)
            return Result.Failure(PollErrors.PollNotFound);

        var response = await _pollrepository.DeletePollAsync(pollId, cancellationToken);
        if(response)
          return Result.Success();

        return Result.Failure(PollErrors.PollDeletionFailed);
    }
    public async Task<Result> publishToggle(int pollId, CancellationToken cancellationToken)
    {
        var isUpdated = await _pollrepository.publishToggle(pollId, cancellationToken); 

        if (isUpdated)
            return Result.Success();
        return Result.Failure(PollErrors.PollPublicationToggleFailed);
    }

}

