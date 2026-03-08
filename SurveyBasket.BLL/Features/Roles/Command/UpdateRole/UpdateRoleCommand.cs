namespace SurveyBasket.BLL.Features.Roles.Command.UpdateRole;
public record UpdateRoleCommand(string RoleId, RoleRequest roleRequest) : IRequest<Result>;
