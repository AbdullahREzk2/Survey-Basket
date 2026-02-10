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

    public static Error DuplicatedEmail =
        new("User.DuplicatedEmail", "Email Already Exists ", StatusCodes.Status409Conflict);

    public static Error EmailNotConfirmed =
        new("User.EmailNotConfirmed", "Email is not Confirmed ", StatusCodes.Status401Unauthorized);

    public static Error DuplicateConfirmation =
        new("User.DuplicateConfirmation", "Email Already Confirmed ", StatusCodes.Status400BadRequest);

    public static Error InvalidCode =
        new("User.InvalidCode", "Invalid Code ", StatusCodes.Status400BadRequest);
}
