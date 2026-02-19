namespace SurveyBasket.BLL.Service;
public class RoleService(IRoleRepository roleRepository) : IRoleService
{
    private readonly IRoleRepository _rolerepository = roleRepository;


    public async Task<IEnumerable<RoleResponse>> getAllRoles (bool?includeDisabled = false,CancellationToken cancellationToken = default)
    {
        var roles = await  _rolerepository.getAllRoles(includeDisabled, cancellationToken);
        return roles.Adapt<IEnumerable<RoleResponse>>();
    }
    public async Task<Result<RoleDetailResponse>> getRoleDetails(string RoleId)
    {
        var role = await _rolerepository.getRoleById(RoleId);

        if (role is not { })
            return Result.Failure<RoleDetailResponse>(RoleErros.RoleNotFound);

        var permissions = await _rolerepository.getRolePermissions(role);

        var response = new RoleDetailResponse(role.Id, role.Name!, role.isDeleted, permissions);

        return Result.Success(response);
    }
    public async Task<Result<RoleDetailResponse>> CreateRole(RoleRequest request)
    {
        var isRoleExist = await _rolerepository.isRoleExist(request.Name);

        if(isRoleExist)
            return Result.Failure<RoleDetailResponse>(RoleErros.RoleAlreadyExist);

        var allowedPermissions = Permissions.GetAllPermissions();

        if(request.Permissions.Except(allowedPermissions).Any())
            return Result.Failure<RoleDetailResponse>(RoleErros.InvalidPermissions);

        var newRole = new ApplicationRole
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

       var createResult = await _rolerepository.CreateRole(newRole);

        if (!createResult.Succeeded)
            return Result.Failure<RoleDetailResponse>(RoleErros.CreationFailed);

        var permissionsResult = await _rolerepository.setPermissionsForRole(newRole, request.Permissions);
        
        if(!permissionsResult.Succeeded)
            return Result.Failure<RoleDetailResponse>(RoleErros.PermissionAssignmentFailed);

        var response = new RoleDetailResponse(newRole.Id, newRole.Name, newRole.isDeleted, request.Permissions);
        return Result.Success(response);

    }
    public async Task<Result> UpdateRole(string RoleId, RoleRequest request)
    {
        var isRoleExist = await _rolerepository.isRoleNameExist(RoleId,request.Name);

        if (isRoleExist)
            return Result.Failure(RoleErros.RoleAlreadyExist);

        var role = await _rolerepository.getRoleById(RoleId);

        if (role is not { })
            return Result.Failure(RoleErros.RoleNotFound);

        var allowedPermissions = Permissions.GetAllPermissions();

        if (request.Permissions.Except(allowedPermissions).Any())
            return Result.Failure(RoleErros.InvalidPermissions);

        using var transaction = await _rolerepository.BeginTransactionAsync();
        try
        {

            role.Name = request.Name;

            var updateResult = await _rolerepository.UpdateRole(role);

            if (!updateResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return Result.Failure(RoleErros.UpdateFailed);
            }

            var currentPermissions =
                await _rolerepository.getRolePermissions(role);

            var newPermissions = 
                request.Permissions.Except(currentPermissions);

            var removedPermissions = 
                currentPermissions.Except(request.Permissions);

           var removedResult = await _rolerepository
                .removePermissionForRole(RoleId, removedPermissions);

            if (!removedResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return Result.Failure(RoleErros.PermissionRemovalFailed);
            }

            var addResult = await _rolerepository.setPermissionsForRole(role, newPermissions);
            if(!addResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return Result.Failure(RoleErros.PermissionAssignmentFailed);
            }

            await transaction.CommitAsync();
            return Result.Success();
        }
        catch
        {
            await transaction.RollbackAsync();
            return Result.Failure(RoleErros.UpdateFailed);
        }

    }
    public async Task<Result> RoleToggleStatus(string RoleId)
    {
        if (await _rolerepository.getRoleById(RoleId) is not { })
            return Result.Failure(RoleErros.RoleNotFound);

        var updatedResult = await _rolerepository.ToggleStatus(RoleId);

        if (!updatedResult)
            return Result.Failure(RoleErros.UpdateFailed);

        return Result.Success();

    }


}
