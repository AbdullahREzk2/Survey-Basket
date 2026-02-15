namespace SurveyBasket.BLL.Contracts.Authantication;
public class ResetPasswordRequestValidation:AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Must be in the Email Format .");

        RuleFor(x => x.Code)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New Password is required.")
            .Matches(RegexPatterns.Password)
            .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
    }

}
