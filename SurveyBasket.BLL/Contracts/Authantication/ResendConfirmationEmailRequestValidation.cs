namespace SurveyBasket.BLL.Contracts.Authantication;
public class ResendConfirmationEmailRequestValidation:AbstractValidator<ResendConfirmationEmailRequest>
{
    public ResendConfirmationEmailRequestValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email Cannot be Empty !");
    }
}
