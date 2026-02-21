namespace SurveyBasket.BLL.Contracts.Users;
public record CreateUserRequest(
    string firstName,
    string lastName,
    string Email,
    string password,
    IList<string> Roles
);
