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
        try
        {
            string confirmationLink = $"{_apiSettings.BaseUrl}/api/users/confirm-email?email={notification.Email}&token={notification.ConfirmationToken}";
            await _emailService.SendWelcomeEmailAsync(notification.Email, notification.FirstName, confirmationLink);
        }
        catch (Exception ex)
        {
            // Log the error but DO NOT throw. 
            // The user transaction has already completed successfully.
            // Throwing here would result in an HTTP 500 to the client despite the user being created.
            // The user can use the "Resend Confirmation Email" feature from the frontend if they don't receive it.
            Console.WriteLine($"[Error] Could not send welcome email to {notification.Email}: {ex.Message}");
        }
    }
}