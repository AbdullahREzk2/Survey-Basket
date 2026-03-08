namespace SurveyBasket.BLL.Features.Polls.Queries.GetAvilablePolls;
public record GetAvaliblePollsQuery : IRequest<Result<IEnumerable<PollResponseDTO>>>;
