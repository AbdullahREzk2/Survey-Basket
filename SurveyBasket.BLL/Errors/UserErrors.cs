namespace SurveyBasket.BLL.Errors;
public static  class UserErrors
{
    public static Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid Email / Password",StatusCodes.Status401Unauthorized);

    public static Error InvalidToken =
        new("User.InvalidToken", "The provided token is invalid or has expired.",StatusCodes.Status401Unauthorized);

    public static Error UserNotFound =
        new("User.NotFound", "The specified user does not exist.", StatusCodes.Status404NotFound);
        
    public static Error RefreshTokenNotFound =
        new("User.RefreshTokenNotFound", "The provided refresh token does not exist.", StatusCodes.Status400BadRequest);
}
