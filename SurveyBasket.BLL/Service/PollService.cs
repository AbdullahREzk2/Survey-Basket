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

    public async Task<IEnumerable<PollResponseDTO>> getAllPollsAsync(CancellationToken cancellationToken)
    {
        var polls = await _pollrepository.getAllPollsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<PollResponseDTO>>(polls);
    }
    public async Task<PollResponseDTO> getPollByIdAsync(int pollId, CancellationToken cancellationToken)
    {
        var poll = await _pollrepository.getPollByIdAsync(pollId,cancellationToken);
        return _mapper.Map<PollResponseDTO>(poll);
    }
    public async Task<PollResponseDTO?> AddPollAsync(PollRequestDTO poll, CancellationToken cancellationToken)
    {
        var pollEntity = _mapper.Map<Poll>(poll);  

        var ifExist = await _pollrepository.SearchPollByTitleAsync(pollEntity.Title, cancellationToken);
        if(ifExist)
            return null;

        var AddedPoll = await _pollrepository.AddPollAsync(pollEntity, cancellationToken);

        return _mapper.Map<PollResponseDTO>(AddedPoll);
    }
    public async Task<PollResponseDTO?> UpdatePollAsync(int pollId, PollRequestDTO poll,CancellationToken cancellationToken)
    {
        var pollEntity = _mapper.Map<Poll>(poll);

        var updatedPoll = await _pollrepository.UpdatePollAsync(pollId, pollEntity,cancellationToken);

        if(updatedPoll == null)
            return null;
        
        return _mapper.Map<PollResponseDTO>(updatedPoll);
    }
    public async Task<bool> DeletePollAsync(int pollId,CancellationToken cancellationToken)
    {
        return await _pollrepository.DeletePollAsync(pollId, cancellationToken);
    }


}

