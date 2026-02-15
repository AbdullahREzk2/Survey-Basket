namespace SurveyBasket.BLL.Contracts.Users;
public class UpdateProfileValidation:AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileValidation()
    {
        RuleFor(x => x.firstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(3,100).WithMessage("FirstName should be between 3,100 ");


        RuleFor(x => x.lastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(3, 100).WithMessage("LastName should be between 3,100 ");
    }
}
