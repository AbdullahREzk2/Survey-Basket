namespace SurveyBasket.API.Extensions;

public static class UserExtension
{
    public static string? GetUserId(this ClaimsPrincipal user)
        => user.FindFirstValue(claimType: ClaimTypes.NameIdentifier);

}
