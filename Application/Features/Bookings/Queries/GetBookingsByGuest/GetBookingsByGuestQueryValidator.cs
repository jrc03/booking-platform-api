using FluentValidation;

namespace Application.Features.Bookings.Queries.GetBookingsByGuest;

public class GetBookingsByGuestQueryValidator : AbstractValidator<GetBookingsByGuestQuery>
{
    public GetBookingsByGuestQueryValidator()
    {
        RuleFor(x => x.GuestId)
            .NotEmpty().WithMessage("Guest ID is required.");
    }
}