namespace SurveyBasket.BLL.IService;
public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
}
