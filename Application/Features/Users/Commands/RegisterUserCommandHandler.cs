using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;
using Domain.Enums;
using Domain.Entities;

namespace Application.Features.Users.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            //TODO FLuent validation
            var newUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = request.Password,

            };

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