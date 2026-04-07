using Application.Interfaces.Email;
using Application.Settings;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.ResendConfirmationEmail
{
    public class ResendConfirmationEmailCommandHandler : IRequestHandler<ResendConfirmationEmailCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ApiSettings _apiSettings;
        private readonly IUnitOfWork _unitOfWork;

        public ResendConfirmationEmailCommandHandler(
            IUserRepository userRepository,
            IEmailService emailService,
            ApiSettings apiSettings,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _apiSettings = apiSettings;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            
            // For security, do not expose if the user exists or is already confirmed via error messages,
            // but for a platform like this, returning gracefully is fine. 
            if (user == null || user.IsEmailConfirmed)
            {
                return; // Nothing to do if user doesn't exist or is already confirmed
            }

            user.GenerateConfirmationToken(); 
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            string confirmationLink = $"{_apiSettings.BaseUrl}/api/users/confirm-email?email={user.Email}&token={user.ConfirmationToken}";

            try 
            {
                await _emailService.SendWelcomeEmailAsync(user.Email, user.FirstName, confirmationLink);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[Error] Could not resend confirmation email to {user.Email}: {ex.Message}");
                throw new InvalidOperationException("Failed to send the confirmation email. Please try again later.");
            }
        }
    }
}