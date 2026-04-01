using System;

namespace Application.Features.Users.DTOs
{
    public record UserResponseDto(Guid Id, string FirstName, string LastName, string Email);
}