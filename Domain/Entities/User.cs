using System;
using Domain.Enums;

namespace Domain.Entities;

public class User
{
    // Parameterless constructor for EF Core
    public User() 
    { 
        IsEmailConfirmed = false;
        GenerateConfirmationToken();
    }

    // Using required and init for initialization
    public  Guid Id { get; init; } = Guid.NewGuid();
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PasswordHash { get; init; }
    
    // Role is not required, defaults to Guest
    public Role Role { get; private set; } = Domain.Enums.Role.Guest; 

    public void AddRole(Role role)
    {
        Role |= role;
    }

    public void RemoveRole(Role role)
    {
        Role &= ~role;
    }

    public bool HasRole(Role role)
    {
        return Role.HasFlag(role);
    }

    // Email Confirmation State
    public bool IsEmailConfirmed { get; private set; }
    public string? ConfirmationToken { get; private set; }
    public DateTime? TokenExpiration { get; private set; }

    public void GenerateConfirmationToken()
    {
        ConfirmationToken = Guid.NewGuid().ToString();
        TokenExpiration = DateTime.UtcNow.AddHours(24);
    }

    public void ConfirmEmail(string token)
    {
        if (IsEmailConfirmed)
            throw new InvalidOperationException("Email is already confirmed");
        
        if (ConfirmationToken != token)
            throw new ArgumentException("Invalid confirmation token");      
        
        if (DateTime.UtcNow > TokenExpiration)
            throw new InvalidOperationException("Confirmation Token has expired");

        IsEmailConfirmed = true;
        ConfirmationToken = null;
        TokenExpiration = null;
    }
}
