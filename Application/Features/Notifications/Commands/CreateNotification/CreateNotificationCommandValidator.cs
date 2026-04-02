using FluentValidation;

namespace Application.Features.Notifications.Commands.CreateNotification
{
    public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
    {
        public CreateNotificationCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required.")
                .MaximumLength(500).WithMessage("Message must not exceed 500 characters.");
        }
    }
}
