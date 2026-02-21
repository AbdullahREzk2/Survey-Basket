namespace SurveyBasket.BLL.Contracts.Users;
public class CreateUserRequestValidation:AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidation()
    {

        RuleFor(x => x.firstName)
            .NotEmpty().WithMessage("FirstName cannot be empty ")
            .Length(3,100).WithMessage("Length must be less than 100");

        RuleFor(x => x.lastName)
            .NotEmpty().WithMessage("LastName cannot be empty ")
            .Length(3,100).WithMessage("Length must be less than 100");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("FirstName cannot be empty ")
            .EmailAddress().WithMessage("Email must be in the email format ");

        RuleFor(x => x.password)
            .NotEmpty().WithMessage("password cannot be empty ")
            .Matches(RegexPatterns.Password)
            .WithMessage("Password must be at least 8 char , 1 upper letter , 1 lower letter,1 number , 1 specail char ");

        RuleFor(x => x.Roles)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Roles)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("you cannot add duplicated role for the same user ")
            .When(x => x.Roles != null);
    }
}
