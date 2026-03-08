namespace SurveyBasket.BLL.Features.Roles.Command.CreateRole;
public record CreateRoleCommand(RoleRequest roleRequest) : IRequest<Result<RoleDetailResponse>>;
