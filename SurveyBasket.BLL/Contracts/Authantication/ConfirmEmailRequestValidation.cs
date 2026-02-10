namespace SurveyBasket.BLL.Contracts.Authantication;
public class ConfirmEmailRequestValidation:AbstractValidator<ConfirmEmailRequestDTO>
{
    public ConfirmEmailRequestValidation()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId cannot be Empty !");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code Cannot be Empty ! ");
    }
}
