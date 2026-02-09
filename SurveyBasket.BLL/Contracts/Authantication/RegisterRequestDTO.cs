namespace SurveyBasket.BLL.Contracts.Authantication;
public record RegisterRequestDTO(
     string Email,
     string Password,
     string FirstName,
     string LastName
 );
