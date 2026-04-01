using Application.Features.Users.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseDto>
    {
        private readonly IGenericRepository<User> _userRepository;

        public GetUserByIdQueryHandler(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                throw new Exception($"User with ID {request.UserId} not found.");
            }

            return new UserResponseDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email
            );
        }
    }
}
