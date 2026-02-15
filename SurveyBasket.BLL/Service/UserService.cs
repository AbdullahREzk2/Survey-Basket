namespace SurveyBasket.BLL.Service;
public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    private readonly UserManager<ApplicationUser> _usermanager = userManager;


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
        var user = await _usermanager.FindByIdAsync(userId);

        user = request.Adapt(user);

        await _usermanager.UpdateAsync(user!);

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


}
