namespace SurveyBasket.BLL.Contracts.Authantication;
public class ForgetPasswordRequestValidation:AbstractValidator<ForgetPasswordRequest>
{
    public ForgetPasswordRequestValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
