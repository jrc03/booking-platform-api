namespace Application.Interfaces.Email
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string toEmail, string firstName, string confirmationLink);
    }
}