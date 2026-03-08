namespace SurveyBasket.BLL.Features.Polls.Commands.PublishToggle;
public record PublishToggleCommand(int pollId) : IRequest<Result>;
