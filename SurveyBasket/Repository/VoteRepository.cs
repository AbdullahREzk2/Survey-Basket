namespace SurveyBasket.DAL.Repository;
public class VoteRepository : IVoteRepository
{
    private readonly ApplicationDBContext _context;

    public VoteRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<bool> HasUserVotedAsync(int pollId, string userId, CancellationToken cancellationToken)
    {
        return await _context.Votes
            .AnyAsync(v => v.PollId == pollId && v.UserId == userId, cancellationToken);
    }
    public async Task<bool> AddVoteAsync(Vote vote, CancellationToken cancellationToken)
    {
        await _context.Votes.AddAsync(vote, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;

    }



}
