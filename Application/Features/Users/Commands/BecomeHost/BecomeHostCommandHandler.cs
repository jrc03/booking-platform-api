using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Users.DTOs;
using Application.Interfaces.Auth;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.BecomeHost
{
    public class BecomeHostCommandHandler : IRequestHandler<BecomeHostCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public BecomeHostCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<UserResponseDto> Handle(BecomeHostCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Invalid user token.");

            var user = await _userRepository.GetByIdAsync(userId) ?? throw new InvalidOperationException("User not found.");
            
            if (user.HasRole(Role.Host))
            {
                throw new InvalidOperationException("User is already a Host.");
            }

            user.AddRole(Role.Host);
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