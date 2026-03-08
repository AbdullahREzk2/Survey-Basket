namespace SurveyBasket.BLL.Features.Polls.Commands.DeletePoll;
public record DeletePollCommand(int pollId) : IRequest<Result>;
