namespace SurveyBasket.BLL.Features.Polls.Queries.GetAllPolls;
public record GetAllPollsQuery : IRequest<Result<IEnumerable<PollResponseDTO>>>;

