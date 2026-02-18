namespace SurveyBasket.DAL.Repository;
public class RoleRepository(RoleManager<ApplicationRole> roleManager,ApplicationDBContext context) : IRoleRepository
{
    private readonly RoleManager<ApplicationRole> _rolemanager = roleManager;
    private readonly ApplicationDBContext _context = context;

    public async Task<IEnumerable<ApplicationRole>> getAllRoles(bool? includeDisabled = false, CancellationToken cancellationToken = default)
    {
       return await _rolemanager.Roles
                   .Where(x => !x.isDeafult && (!x.isDeleted || (includeDisabled.HasValue && includeDisabled.Value)))
                   .ToListAsync(cancellationToken);
    }

    public async Task<ApplicationRole?> getRoleById(string RoleId)
    {
        return await _rolemanager.FindByIdAsync(RoleId);
    }

    public async Task<IEnumerable<string>> getRolePermissions (ApplicationRole role)
    {
        var permissions = await _rolemanager.GetClaimsAsync(role);
        return permissions.Select(x => x.Value);
    }

    public async Task<bool> isRoleExist(string Name)
    {
        return await _rolemanager.RoleExistsAsync(Name);
    }

    public async Task<IdentityResult> CreateRole(ApplicationRole role)
    {
        return await _rolemanager.CreateAsync(role);

    }

    public async Task<IdentityResult> setPermissionsForRole(ApplicationRole role, IEnumerable<string> permissions)
    {
        var Permission = permissions.Select(x => new IdentityRoleClaim<string>
        {
            ClaimType = Permissions.Type,
            ClaimValue = x,
            RoleId = role.Id
        });
        await _context.AddRangeAsync(Permission);
        await _context.SaveChangesAsync();

        return IdentityResult.Success;
    }

}
