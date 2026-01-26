namespace SurveyBasket.BLL.IService;
public interface IVoteService
{
    Task<Result> AddVoteAsync(int pollId,string userId , VoteRequest voteRequest, CancellationToken cancellationToken);
}
