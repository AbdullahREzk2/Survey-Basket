namespace SurveyBasket.API.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpcontextaccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpcontextaccessor = httpContextAccessor;
    }

    public string? UserId =>
         _httpcontextaccessor.HttpContext?.User.GetUserId();
            
    


}
