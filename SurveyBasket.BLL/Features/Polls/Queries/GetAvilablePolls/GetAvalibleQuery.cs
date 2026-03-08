namespace SurveyBasket.BLL.Features.Polls.Queries.GetAvilablePolls;
public record GetAvalibleQuery : IRequest<Result<IEnumerable<PollResponseDTO>>>;
