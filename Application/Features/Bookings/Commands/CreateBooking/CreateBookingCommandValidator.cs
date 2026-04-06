using FluentValidation;
using System;

namespace Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty()
            .WithMessage("Property ID is required.");

        RuleFor(x => x.GuestId)
            .NotEmpty()
            .WithMessage("Guest ID is required.");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Start Date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End Date must be strictly after Start Date.");
    }
}