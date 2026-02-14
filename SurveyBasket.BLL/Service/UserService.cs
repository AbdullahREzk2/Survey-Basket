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

}
