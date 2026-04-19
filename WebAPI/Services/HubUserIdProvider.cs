using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Services;

public class HubUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst("id")?.Value
               ?? connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? connection.User?.FindFirst("sub")?.Value;
    }
}
