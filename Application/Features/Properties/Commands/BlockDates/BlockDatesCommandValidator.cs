using FluentValidation;

namespace Application.Features.Properties.Commands.BlockDates;

public class BlockDatesCommandValidator : AbstractValidator<BlockDatesCommand>
{
    public BlockDatesCommandValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty()
            .WithMessage("Property ID is required.");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Start Date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End Date must be strictly after Start Date.");
    }
}
