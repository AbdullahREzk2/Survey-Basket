namespace SurveyBasket.BLL.IService;
public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
    Task<Result> UpdateUserProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken=default);
    Task<Result<UserResponse>> GetUserDetailsAsync(string userId);
    Task<Result<UserResponse>> createUserAsync(CreateUserRequest request,CancellationToken cancellationToken=default);
    Task<Result> updateUserAsync(string userId , UpdateUserRequest request,CancellationToken cancellationToken=default);
    Task<Result> toggleStatusAsync(string userId);
    Task<Result> unlockUserAsync(string userId);
}
