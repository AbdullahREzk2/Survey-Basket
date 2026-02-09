using SurveyBasket.BLL.Abstractions.Consts;

namespace SurveyBasket.BLL.Contracts.Authantication;
public class RegisterRequestValidation:AbstractValidator<RegisterRequestDTO>
{
    public RegisterRequestValidation()
    {
        RuleFor(x=>x.Email)
            .NotEmpty().WithMessage("Email Should Not be Empty !")
            .EmailAddress().WithMessage("Should be in the Email Format !");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password Cannot be Empty !")
            .Matches(RegexPatterns.Password)
            .WithMessage("Password should at least 8 digits , contains at Least ( 1 LowerLetter,1 UpperLetter ,1 digit,1 special character )");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName Cannot be empty !")
            .MaximumLength(100).WithMessage("FirstName Should be Less than 100 !");

        RuleFor(x=>x.LastName)
            .NotEmpty().WithMessage("LastName Cannot be Empty !")
            .MaximumLength(100).WithMessage("LastName Should be less than 100 !");

    }


}
