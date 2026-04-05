using FluentValidation;

namespace Application.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(x => x.HostId)
                .NotEmpty().WithMessage("Host ID is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(5).WithMessage("Title must be at least 5 characters.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(20).WithMessage("Please provide a more detailed description (at least 20 characters).")
                .MaximumLength(1000).WithMessage("Description is too long (maximum 1000 characters).");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");
           
            RuleFor(x => x.PricePerNight)
                .GreaterThan(0).WithMessage("Price per night must be greater than zero.")
                .LessThan(10000).WithMessage("Price per night exceeds the maximum allowed limit ($10,000).");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be at least 1 person.")
                .LessThanOrEqualTo(30).WithMessage("Capacity cannot exceed 30 people for a single property.");
        }
    }
}