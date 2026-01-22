namespace SurveyBasket.API;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpcontextaccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpcontextaccessor = httpContextAccessor;
    }

    public string? UserId=>
         _httpcontextaccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);
    


}
