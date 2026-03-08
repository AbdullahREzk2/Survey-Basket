namespace SurveyBasket.BLL.Features.Polls.Commands.PublishToggle;
public record PollPublishToggleCommand(int pollId) : IRequest<Result>;
