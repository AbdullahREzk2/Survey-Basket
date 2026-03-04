namespace SurveyBasket.BLL.Service;

public class UserService(IUserRepository userRepository, IRoleService roleService) : IUserService
{
    private readonly IUserRepository _userrepository = userRepository;
    private readonly IRoleService _roleservice = roleService;

    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
    {
        var user = await _userrepository.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);

        return Result.Success(user.Adapt<UserProfileResponse>());
    }

    public async Task<Result> UpdateUserProfileAsync(string userId, UpdateProfileRequest request)
    {
        await _userrepository.UpdateUserProfileAsync(userId, request.firstName, request.lastName);
        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userrepository.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var result = await _userrepository.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userrepository.GetUsersWithRolesAsync(cancellationToken);
        return users.Adapt<IEnumerable<UserResponse>>();
    }

    public async Task<Result<UserResponse>> GetUserDetailsAsync(string userId)
    {
        var user = await _userrepository.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        var userRoles = await _userrepository.GetRolesAsync(user);
        var response = (user, userRoles).Adapt<UserResponse>();

        return Result.Success(response);
    }

    public async Task<Result<UserResponse>> createUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userrepository.EmailExistsAsync(request.Email))
            return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail);

        var allowedRoles = await _roleservice.getAllRoles(cancellationToken: cancellationToken);
        var allowedRoleNames = allowedRoles.Select(x => x.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (request.Roles.Any(r => !allowedRoleNames.Contains(r)))
            return Result.Failure<UserResponse>(UserErrors.InvalidRoles);

        var user = request.Adapt<ApplicationUser>();
        user.UserName = request.Email;
        user.EmailConfirmed = true;

        var result = await _userrepository.CreateAsync(user, request.password);
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var addRoleResult = await _userrepository.AddToRolesAsync(user, request.Roles);
        if (!addRoleResult.Succeeded)
        {
            var error = addRoleResult.Errors.First();
            return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var response = (user, request.Roles).Adapt<UserResponse>();
        return Result.Success(response);
    }

    public async Task<Result> updateUserAsync(string userId, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userrepository.EmailExistsForOtherUserAsync(request.Email, userId, cancellationToken))
            return Result.Failure(UserErrors.DuplicatedEmail);

        var allowedRoles = await _roleservice.getAllRoles(cancellationToken: cancellationToken);
        if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
            return Result.Failure(UserErrors.InvalidRoles);

        var user = await _userrepository.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        user = request.Adapt(user);
        user.UserName = request.Email;
        user.NormalizedEmail = request.Email.ToUpper();

        var result = await _userrepository.UpdateAsync(user);
        if (result.Succeeded)
        {
            await _userrepository.DeleteUserRolesAsync(userId, cancellationToken);
            await _userrepository.AddToRolesAsync(user, request.Roles);
            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> toggleStatusAsync(string userId)
    {
        var user = await _userrepository.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        user.isDisabled = !user.isDisabled;
        var result = await _userrepository.UpdateAsync(user);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> unlockUserAsync(string userId)
    {
        var user = await _userrepository.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var result = await _userrepository.SetLockoutEndDateAsync(user, null);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
}