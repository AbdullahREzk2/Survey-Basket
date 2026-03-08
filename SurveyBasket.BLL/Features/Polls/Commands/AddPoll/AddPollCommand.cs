namespace SurveyBasket.BLL.Features.Polls.Commands.AddPoll;
public record AddPollCommand(PollRequestDTO poll) : IRequest<Result<PollResponseDTO>>;
