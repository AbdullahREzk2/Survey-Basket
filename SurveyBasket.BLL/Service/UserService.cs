namespace SurveyBasket.BLL.Service;
public class UserService(UserManager<ApplicationUser> userManager , ApplicationDBContext context,IRoleService roleService) : IUserService
{
    private readonly UserManager<ApplicationUser> _usermanager = userManager;
    private readonly ApplicationDBContext _context = context;
    private readonly IRoleService _roleservice = roleService;

    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
    {
        var user = await _usermanager.Users
            .Where(x=>x.Id == userId)
            .ProjectToType<UserProfileResponse>()
            .SingleAsync();

        return Result.Success(user);
    }
    public async Task<Result> UpdateUserProfileAsync(string userId, UpdateProfileRequest request)
    {
        //var user = await _usermanager.FindByIdAsync(userId);

        //user = request.Adapt(user);

        //await _usermanager.UpdateAsync(user!);

        await _usermanager.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(p => p.firstName, request.firstName)
                .SetProperty(p => p.lastName, request.lastName)
            );
        return Result.Success();
    }
    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _usermanager.FindByIdAsync(userId);

        var result = await _usermanager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if(result.Succeeded)
        return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description,StatusCodes.Status400BadRequest));
    }
    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default) =>
        await (
            from u in _context.Users
            join ur in _context.UserRoles
                on u.Id equals ur.UserId
            join r in _context.Roles
                on ur.RoleId equals r.Id into roles
            where !roles.Any(x => x.Name == defaultRoles.Member)
            select new
            {
                u.Id,
                u.firstName,
                u.lastName,
                u.Email,
                u.isDisabled,
                Roles = roles.Select(x => x.Name!)
            }
        )
        .GroupBy(u => new { u.Id, u.firstName, u.lastName, u.Email, u.isDisabled })
        .Select(u => new UserResponse(
            u.Key.Id,
            u.Key.firstName,
            u.Key.lastName,
            u.Key.Email!,
            u.Key.isDisabled,
            u.SelectMany(x => x.Roles).Distinct()
         ))
        .ToListAsync(cancellationToken); 
    public async Task<Result<UserResponse>> GetUserDetailsAsync(string userId)
    {
        if (await _usermanager.FindByIdAsync(userId) is not { } user)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        var userRoles = await _usermanager.GetRolesAsync(user);

        var response = (user,userRoles).Adapt<UserResponse>();

        return Result.Success(response);
    }
    public async Task<Result<UserResponse>> createUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (await _usermanager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
            return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail);

        var allowedRoles = await _roleservice.getAllRoles(cancellationToken:cancellationToken);

        var allowedRoleNames = allowedRoles.Select(x => x.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (request.Roles.Any(r => !allowedRoleNames.Contains(r)))
            return Result.Failure<UserResponse>(UserErrors.InvalidRoles);

        var user = request.Adapt<ApplicationUser>();
        user.UserName = request.Email;
        user.EmailConfirmed = true;

        var result = await _usermanager.CreateAsync(user, request.password);
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var addRoleResult = await _usermanager.AddToRolesAsync(user, request.Roles);
        if (!addRoleResult.Succeeded)
        {
            var error = addRoleResult.Errors.First();
            return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var response = (user,request.Roles).Adapt<UserResponse>();

        return Result.Success(response);
    }
    public async Task<Result> updateUserAsync(string userId, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var emailExists = await _usermanager.Users.AnyAsync(x=>x.Email == request.Email && x.Id !=  userId ,cancellationToken);

        if (emailExists)
            return Result.Failure(UserErrors.DuplicatedEmail);

        var allowedRoles = await _roleservice.getAllRoles(cancellationToken:cancellationToken);

        if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
            return Result.Failure(UserErrors.InvalidRoles);

        if(await _usermanager.FindByIdAsync(userId) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        user = request.Adapt(user);
        user.UserName = request.Email;
        user.NormalizedEmail = request.Email.ToUpper();

        var result = await _usermanager.UpdateAsync(user);
        if (result.Succeeded)
        {
            await _context.UserRoles
                .Where(x => x.UserId == userId)
                .ExecuteDeleteAsync(cancellationToken);

            await _usermanager.AddToRolesAsync(user, request.Roles);
            return Result.Success();
        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> toggleStatusAsync(string userId)
    {
        if(await _usermanager.FindByIdAsync(userId) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        user.isDisabled = !user.isDisabled;
        var result = await _usermanager.UpdateAsync(user);

        if(result.Succeeded) 
        return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> unlockUserAsync(string userId)
    {
        if (await _usermanager.FindByIdAsync(userId) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        var result = await _usermanager.SetLockoutEndDateAsync(user, null);

        if(result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
}
