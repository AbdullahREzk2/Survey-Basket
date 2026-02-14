namespace SurveyBasket.DAL.IRepository;
public interface IPollRepository
{
    Task<IEnumerable<Poll>> getAllPollsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Poll>> getAvailblePollsAsync(CancellationToken cancellationToken);
    Task<Poll?> getAvailblePollAsync(int pollId,CancellationToken cancellationToken);
    Task<Poll> getPollByIdAsync(int pollId,CancellationToken cancellationToken);
    Task<Poll> AddPollAsync(Poll poll,CancellationToken cancellationToken);
    Task<Poll?> UpdatePollAsync(int pollId,Poll poll,CancellationToken cancellationToken);
    Task<bool> DeletePollAsync(int pollId,CancellationToken cancellationToken);
    Task<bool> SearchPollByTitleAsync(string title,CancellationToken cancellationToken);
    Task<bool> publishToggle(int pollId,CancellationToken cancellationToken);
    Task<IEnumerable<Poll>> getTodayPolls(CancellationToken cancellationToken);
    Task<Poll> getTodayPoll(int? PollId,CancellationToken cancellationToken); 
}
