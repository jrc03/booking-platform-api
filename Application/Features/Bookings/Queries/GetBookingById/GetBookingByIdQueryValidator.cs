using FluentValidation;

namespace Application.Features.Bookings.Queries.GetBookingById;

public class GetBookingByIdQueryValidator : AbstractValidator<GetBookingByIdQuery>
{
    public GetBookingByIdQueryValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID is required.");
    }
}