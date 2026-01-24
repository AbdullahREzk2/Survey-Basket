namespace SurveyBasket.BLL.Errors;
public static  class UserErrors
{
    public static Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid Email / Password");

    public static Error InvalidToken =
        new("User.InvalidToken", "The provided token is invalid or has expired.");

    public static Error UserNotFound =
        new("User.NotFound", "The specified user does not exist.");
        
    public static Error RefreshTokenNotFound =
        new("User.RefreshTokenNotFound", "The provided refresh token does not exist.");
}
