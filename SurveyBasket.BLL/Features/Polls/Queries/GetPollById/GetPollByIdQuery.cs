namespace SurveyBasket.BLL.Features.Polls.Queries.GetPollById;
public record GetPollByIdQuery(int PollId) : IRequest<Result<PollResponseDTO>>;
