namespace SurveyBasket.BLL.Features.Roles.Command.UpdateRole;
public class UpdateRoleCommandHandler(IRoleRepository roleRepository) : IRequestHandler<UpdateRoleCommand, Result>
{
    private readonly IRoleRepository _rolerepository = roleRepository;

    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var isRoleExist = await _rolerepository.isRoleNameExist(request.RoleId, request.roleRequest.Name);

        if (isRoleExist)
            return Result.Failure(RoleErros.RoleAlreadyExist);

        var role = await _rolerepository.getRoleById(request.RoleId);

        if (role is not { })
            return Result.Failure(RoleErros.RoleNotFound);

        var allowedPermissions = Permissions.GetAllPermissions();

        if (request.roleRequest.Permissions.Except(allowedPermissions).Any())
            return Result.Failure(RoleErros.InvalidPermissions);

        using var transaction = await _rolerepository.BeginTransactionAsync();
        try
        {

            role.Name = request.roleRequest.Name;

            var updateResult = await _rolerepository.UpdateRole(role);

            if (!updateResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return Result.Failure(RoleErros.UpdateFailed);
            }

            var currentPermissions =
                await _rolerepository.getRolePermissions(role);

            var newPermissions =
                request.roleRequest.Permissions.Except(currentPermissions);

            var removedPermissions =
                currentPermissions.Except(request.roleRequest.Permissions);

            var removedResult = await _rolerepository
                 .removePermissionForRole(request.RoleId, removedPermissions);

            if (!removedResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return Result.Failure(RoleErros.PermissionRemovalFailed);
            }

            var addResult = await _rolerepository.setPermissionsForRole(role, newPermissions);
            if (!addResult.Succeeded)
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
}
