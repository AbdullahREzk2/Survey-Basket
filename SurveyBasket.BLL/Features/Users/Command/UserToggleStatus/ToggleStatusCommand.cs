namespace SurveyBasket.BLL.Features.Users.Command.UserToggleStatus;
public record ToggleStatusCommand(string userId) : IRequest<Result>;
