namespace SurveyBasket.BLL.Contracts.Authantication;
public class loginRequestValidation:AbstractValidator<loginRequestDTO>
{
    public loginRequestValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Enter the correct format of email");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long");
    }
}
