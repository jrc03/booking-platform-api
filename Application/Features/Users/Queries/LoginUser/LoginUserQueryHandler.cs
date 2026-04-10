using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Application.Features.Users.DTOs;
using Application.Interfaces.Auth;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, AuthenticationResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserQueryHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthenticationResponseDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email) ?? throw new InvalidCredentialException("Invalid Credentials");

            if (!user.IsEmailConfirmed)
                throw new UnauthorizedAccessException("Email not confirmed");

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                throw new InvalidCredentialException("Invalid Credentials");

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResponseDto
            (
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                token
            );


        }
    }
}