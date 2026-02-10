namespace SurveyBasket.BLL.Contracts.Authantication;
public record ConfirmEmailRequestDTO(
 
    string UserId,
    string Code
 );
