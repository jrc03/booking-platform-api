using Application.Features.Users.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponseDto>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IGenericRepository<User> userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                throw new Exception($"User with ID {request.UserId} not found.");
            }

            // Usamos el método de dominio encapsulado
            user.UpdateProfile(request.FirstName, request.LastName);

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UserResponseDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email
            );
        }
    }
}
