namespace SurveyBasket.DAL.Repository;
public class PollRepository : IPollRepository
{
    private readonly ApplicationDBContext _dbcontext;

    public PollRepository(ApplicationDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public async Task<IEnumerable<Poll>> getAllPollsAsync(CancellationToken cancellationToken)
    {
        var polls = await _dbcontext.Polls.AsNoTracking().ToListAsync(cancellationToken);

        return polls;
    }
    public async Task<IEnumerable<Poll>> getAvailblePollsAsync(CancellationToken cancellationToken)
    {
        return await _dbcontext.Polls
            .Where (
            p=>p.isPublished 
            && p.startDate <= DateOnly.FromDateTime(DateTime.UtcNow) 
            && p.endDate >=DateOnly.FromDateTime (DateTime.UtcNow)
            )
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<Poll?> getAvailblePollAsync(int pollId, CancellationToken cancellationToken)
    {
        return await _dbcontext.Polls
            .Where(
            p => p.PollId == pollId 
            && p.isPublished 
            && p.startDate <= DateOnly.FromDateTime(DateTime.UtcNow) 
            && p.endDate >= DateOnly.FromDateTime(DateTime.UtcNow)
            )
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Poll> getPollByIdAsync(int pollId,CancellationToken cancellationToken)
    {
        var poll = await _dbcontext.Polls
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PollId == pollId,cancellationToken);

        return poll!;
    }
    public async Task<Poll> AddPollAsync(Poll poll,CancellationToken cancellationToken)
    {
        await _dbcontext.Polls.AddAsync(poll,cancellationToken);
        await _dbcontext.SaveChangesAsync(cancellationToken);
        return poll;
    }
    public async Task<Poll?> UpdatePollAsync(int pollId, Poll poll,CancellationToken cancellationToken)
    {
        var exsistingPoll = await _dbcontext.Polls
            .FirstOrDefaultAsync(p => p.PollId == pollId, cancellationToken);

        if (exsistingPoll == null)
            return null;

        exsistingPoll.Title = poll.Title;
        exsistingPoll.Description = poll.Description;
        exsistingPoll.isPublished = poll.isPublished;
        exsistingPoll.startDate = poll.startDate;
        exsistingPoll.endDate = poll.endDate;
        

        await _dbcontext.SaveChangesAsync(cancellationToken);
        return exsistingPoll;

    }
    public async Task<bool> DeletePollAsync(int pollId,CancellationToken cancellationToken)
    {
        var poll= await _dbcontext.Polls.FirstOrDefaultAsync(p => p.PollId == pollId, cancellationToken);

        if (poll == null)
            return false;

        _dbcontext.Polls.Remove(poll);
         await _dbcontext.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<bool> SearchPollByTitleAsync(string title,CancellationToken cancellationToken)
    {
        return await _dbcontext.Polls
            .AsNoTracking()
            .AnyAsync(p => p.Title == title, cancellationToken);
    }
    public async Task<bool> publishToggle(int pollId, CancellationToken cancellationToken)
    {
        var poll = await _dbcontext.Polls.FirstOrDefaultAsync(p => p.PollId == pollId, cancellationToken);

        if (poll == null)
            return false;

        poll.isPublished = !poll.isPublished;

        await _dbcontext.SaveChangesAsync(cancellationToken);
        return true;

    }

}
