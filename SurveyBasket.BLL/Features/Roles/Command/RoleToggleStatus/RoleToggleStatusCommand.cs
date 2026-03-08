namespace SurveyBasket.BLL.Features.Roles.Command.RoleToggleStatus;
public record RoleToggleStatusCommand(string RoleId) : IRequest<Result>;
