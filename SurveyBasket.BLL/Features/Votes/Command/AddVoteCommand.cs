namespace SurveyBasket.BLL.Features.Votes.Command;
public record AddVoteCommand(int pollId, string userId, VoteRequest voteRequest) : IRequest<Result>;
