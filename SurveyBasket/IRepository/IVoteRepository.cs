namespace SurveyBasket.DAL.IRepository;
public interface IVoteRepository
{
    Task<bool> HasUserVotedAsync(int pollId, string userId, CancellationToken cancellationToken);
}
