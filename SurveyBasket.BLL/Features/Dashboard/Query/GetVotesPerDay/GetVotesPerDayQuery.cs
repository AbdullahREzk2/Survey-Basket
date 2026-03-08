namespace SurveyBasket.BLL.Features.Dashboard.Query.GetVotesPerDay;
public record GetVotesPerDayQuery(int pollId) : IRequest<Result<IEnumerable<VotesPerDayResponse>>>;
