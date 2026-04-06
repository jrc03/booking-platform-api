using MediatR;

namespace Application.Features.Users.Events;

public record UserRegisteredEvent(Guid UserId, string Email, string FirstName, string ConfirmationToken) : INotification;