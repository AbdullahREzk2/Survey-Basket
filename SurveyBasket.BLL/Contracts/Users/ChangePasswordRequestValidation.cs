namespace SurveyBasket.BLL.Contracts.Users;
public class ChangePasswordRequestValidation:AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidation()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required.");


        RuleFor(x => x.NewPassword)
           .NotEmpty().WithMessage("Password Cannot be Empty !")
           .Matches(RegexPatterns.Password)
           .WithMessage("Password should at least 8 digits , contains at Least ( 1 LowerLetter,1 UpperLetter ,1 digit,1 special character )")
           .NotEqual(x=>x.CurrentPassword) .WithMessage("New Password cannot be same as the current Password ! ");
            
    }
}
