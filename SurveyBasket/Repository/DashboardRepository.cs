namespace SurveyBasket.DAL.Repository;
public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDBContext _context;

    public DashboardRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<string?> GetPollTitleAsync(int pollId, CancellationToken cancellationToken = default)
    {
        return await _context.Polls
            .Where(p => p.PollId == pollId)
            .Select(p => p.Title)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<IEnumerable<VoterDetails>>GetVoterDetailsAsync(int pollId, CancellationToken cancellationToken = default)
    {
        return await _context.Votes
            .Where(v => v.PollId == pollId)
            .Select(v => new VoterDetails
            {
                VoteId = v.voteId,
                VoterName = v.User.firstName + " " + v.User.lastName,
                VoteDate = v.submittedOn
            })
            .ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<QuestionAnswerDTO>>GetQuestionAnswersAsync(int pollId, CancellationToken cancellationToken = default)
    {
        return await _context.voteAnswers
            .Where(va => va.Vote.PollId == pollId)
            .Select(va => new QuestionAnswerDTO
            {
                VoteId = va.VoteId,
                QuestionContent = va.question.Content,
                AnswerContent = va.Answer.Content
            })
            .ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<VotesPerDayDTO>>GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default)
    {
        return await _context.Votes
            .Where(v => v.PollId == pollId)
            .GroupBy(v => v.submittedOn.Date)
            .Select(g => new VotesPerDayDTO(
                DateOnly.FromDateTime(g.Key),
                g.Count()
            ))
            .ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<QuestionVoteDTO>> getVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
    {
        return await _context.voteAnswers
            .Where(v=>v.Vote.PollId == pollId)
            .GroupBy(v => new
            {
                v.QuestionId,
                v.question.Content
            })
            .Select(g=> new QuestionVoteDTO(
            
                 g.Key.Content,
                 g.Count()
            ))
            .ToListAsync(cancellationToken);
    }


}
