namespace SurveyBasket.BLL.Features.Polls.Commands.AddPoll;

public class AddPollCommandValidator : AbstractValidator<AddPollCommand>
{
    public AddPollCommandValidator()
    {
        RuleFor(x => x.poll.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.poll.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.poll.startDate)
            .LessThanOrEqualTo(x => x.poll.endDate)
            .WithMessage("Start date must be before or equal to end date.");

        RuleFor(x => x.poll.endDate)
            .GreaterThanOrEqualTo(x => x.poll.startDate)
            .WithMessage("End date must be after or equal to start date.");
    }
}