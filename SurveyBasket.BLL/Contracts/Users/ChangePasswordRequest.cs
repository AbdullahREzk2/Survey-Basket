namespace SurveyBasket.BLL.Contracts.Users;
public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
    );
