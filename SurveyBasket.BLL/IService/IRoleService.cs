namespace SurveyBasket.BLL.IService;
public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> getAllRoles(bool? includeDisabled = false,CancellationToken cancellationToken=default);
    Task<Result<RoleDetailResponse>> getRoleDetails(string RoleId);
    Task<Result<RoleDetailResponse>> CreateRole(RoleRequest request);
}
