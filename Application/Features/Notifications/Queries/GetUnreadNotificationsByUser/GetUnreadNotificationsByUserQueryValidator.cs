using FluentValidation;

namespace Application.Features.Notifications.Queries.GetUnreadNotificationsByUser
{
    public class GetUnreadNotificationsByUserQueryValidator : AbstractValidator<GetUnreadNotificationsByUserQuery>
    {
        public GetUnreadNotificationsByUserQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
