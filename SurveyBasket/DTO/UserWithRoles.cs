namespace SurveyBasket.DAL.DTO;
public record UserWithRoles(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsDisabled
    , IEnumerable<string> Roles
    );
