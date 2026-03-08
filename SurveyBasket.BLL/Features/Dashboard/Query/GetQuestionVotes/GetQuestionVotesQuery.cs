namespace SurveyBasket.BLL.Features.Dashboard.Query.GetQuestionVotes;
public record GetQuestionVotesQuery(int pollId) : IRequest<Result<IEnumerable<QuestionVoteResponse>>>;
