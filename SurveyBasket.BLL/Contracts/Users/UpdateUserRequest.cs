namespace SurveyBasket.BLL.Contracts.Users;
public record UpdateUserRequest(
    string firstName,
    string lastName,
    string Email,
    IList<string> Roles
);
