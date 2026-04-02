using Domain.Interfaces;
using MediatR;
using Domain.Enums;
using Domain.Entities;
using Application.Features.Users.DTOs;
using Application.Interfaces.Auth;

namespace Application.Features.Users.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

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


            return new UserResponseDto(
                newUser.Id,
                newUser.FirstName,
                newUser.LastName,
                newUser.Email
            );
        }
    }
}