using Application.Features.Users.Events;
using Application.Interfaces.Email;
using Application.Settings;
using MediatR;

namespace Application.Features.Users.EventHandlers;

public class SendWelcomeEmailHandler : INotificationHandler<UserRegisteredEvent>
{
    private readonly IEmailService _emailService;
    private readonly ApiSettings _apiSettings;

    public SendWelcomeEmailHandler(IEmailService emailService, ApiSettings apiSettings)
    {
        _emailService = emailService;
        _apiSettings = apiSettings;
    }

    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        string confirmationLink = $"{_apiSettings.BaseUrl}/api/users/confirm-email?email={notification.Email}&token={notification.ConfirmationToken}";

        await _emailService.SendWelcomeEmailAsync(notification.Email, notification.FirstName, confirmationLink);
    }
}