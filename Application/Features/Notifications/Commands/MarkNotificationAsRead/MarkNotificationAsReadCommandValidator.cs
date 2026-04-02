using FluentValidation;

namespace Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommandValidator : AbstractValidator<MarkNotificationAsReadCommand>
    {
        public MarkNotificationAsReadCommandValidator()
        {
            RuleFor(x => x.NotificationId)
                .NotEmpty().WithMessage("Notification ID is required.");
        }
    }
}
