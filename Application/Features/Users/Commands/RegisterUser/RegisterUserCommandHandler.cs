using Domain.Interfaces;
using MediatR;
using Domain.Enums;
using Domain.Entities;
using Application.Features.Users.DTOs;
using Application.Interfaces.Auth;
using Application.Interfaces.Email;
using Application.Features.Users.Events;

namespace Application.Features.Users.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPublisher _publisher;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IPublisher publisher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _publisher = publisher;
        }

        public async Task<UserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("This email is already registered in the platform.");
            }

            var newUser = User.Create(
                request.FirstName,
                request.LastName,
                request.Email,
                _passwordHasher.HashPassword(request.Password)
            );

            if (request.IsHost)
            {
                newUser.AddRole(Role.Host);
            }

            _userRepository.Add(newUser);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
           
            await _publisher.Publish(new UserRegisteredEvent(
                newUser.Id,
                newUser.Email,
                newUser.FirstName,
                newUser.ConfirmationToken!), cancellationToken);


            return new UserResponseDto(
                newUser.Id,
                newUser.FirstName,
                newUser.LastName,
                newUser.Email
            );
        }
    }
}