namespace SurveyBasket.DAL.IRepository;
public interface IRoleRepository
{
    Task<ApplicationRole?> getRoleById(string RoleId);
    Task<IEnumerable<ApplicationRole>> getAllRoles(bool? includeDisabled = false,CancellationToken cancellationToken=default);
    Task <IEnumerable<string>> getRolePermissions(ApplicationRole role);
    Task<bool> isRoleExist(string Name);
    Task<IdentityResult> CreateRole(ApplicationRole role);
    Task<IdentityResult> setPermissionsForRole(ApplicationRole role, IEnumerable<string> permissions);
    Task<bool> isRoleNameExist(string RoleId , string Name);
    Task<IdentityResult> UpdateRole(ApplicationRole role);
    Task<IdentityResult> removePermissionForRole(string roleId, IEnumerable<string> removedPermissions);

    Task<IDbContextTransaction> BeginTransactionAsync();

}
