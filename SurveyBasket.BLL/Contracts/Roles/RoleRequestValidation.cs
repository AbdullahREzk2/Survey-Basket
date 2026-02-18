namespace SurveyBasket.BLL.Contracts.Roles;
public class RoleRequestValidation:AbstractValidator<RoleRequest>
{
    public RoleRequestValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .Length(3,200).WithMessage("Length Must be between 3 ,200 ");

        RuleFor(x => x.Permissions)
            .NotNull()
            .NotEmpty();

        RuleFor(x=>x.Permissions)
            .Must(x=>x.Distinct().Count()==x.Count)
            .WithMessage("you can't have duplicate permissions")
            .When(x=> x.Permissions!=null);
    }
}
