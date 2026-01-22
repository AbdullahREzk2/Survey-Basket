namespace SurveyBasket.BLL.Contracts.Polls;
public class PollRequestValidation:AbstractValidator<PollRequestDTO>
{
    public PollRequestValidation()
    {
        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(p => p.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(p => p.startDate)
            .LessThanOrEqualTo(p => p.endDate).WithMessage("Start date must be before or equal to end date.");

        RuleFor(p => p.endDate)
            .GreaterThanOrEqualTo(p => p.startDate).WithMessage("End date must be after or equal to start date.");
    }
}
