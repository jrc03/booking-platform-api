namespace Application.Interfaces.Auth;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}