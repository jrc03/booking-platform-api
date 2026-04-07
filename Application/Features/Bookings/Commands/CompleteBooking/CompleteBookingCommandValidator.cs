using FluentValidation;

namespace Application.Features.Bookings.Commands.CompleteBooking
{
    public class CompleteBookingCommandValidator : AbstractValidator<CompleteBookingCommand>
    {
        public CompleteBookingCommandValidator()
        {
            RuleFor(x => x.BookingId)
                .NotEmpty().WithMessage("Booking ID is required.");
        }
    }
}