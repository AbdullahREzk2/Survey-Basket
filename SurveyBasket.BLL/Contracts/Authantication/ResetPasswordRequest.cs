namespace SurveyBasket.BLL.Contracts.Authantication;
public record ResetPasswordRequest(
    string Email,
    string Code,
    string NewPassword

    );
