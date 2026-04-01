using FluentValidation;

namespace Application.Features.Bookings.Commands.CancelBooking
{
    public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
    {
        public CancelBookingCommandValidator()
        {
            RuleFor(x => x.BookingId)
                .NotEmpty().WithMessage("Booking ID is required.");

            RuleFor(x => x.GuestId)
                .NotEmpty().WithMessage("Guest ID is required.");
        }
    }
}
