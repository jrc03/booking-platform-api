using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.Users.DTOs
{
    public record AuthenticationResponseDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Token
);
}