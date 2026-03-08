namespace SurveyBasket.BLL.Features.Polls.Commands.UpdatePoll;
public record UpdatePollCommand(int pollId, PollRequestDTO poll) : IRequest<Result<PollResponseDTO>>;
