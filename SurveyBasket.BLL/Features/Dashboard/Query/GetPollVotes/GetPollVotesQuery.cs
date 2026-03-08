namespace SurveyBasket.BLL.Features.Dashboard.Query.GetPollVotes;
public record GetPollVotesQuery (int pollId) : IRequest<Result<PollVotesResponse>>;
