namespace SurveyBasket.DAL.IRepository;
public interface IRoleRepository
{
    Task<ApplicationRole?> getRoleById(string RoleId);
    Task<IEnumerable<ApplicationRole>> getAllRoles(bool? includeDisabled = false,CancellationToken cancellationToken=default);
    Task <IEnumerable<string>> getRolePermissions(ApplicationRole role);
    Task<bool> isRoleExist(string Name);
    Task<IdentityResult> CreateRole(ApplicationRole role);
    Task<IdentityResult> setPermissionsForRole(ApplicationRole role, IEnumerable<string> permissions);

}
