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
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    public static User Create(string firstName, string lastName, string email, string passwordHash)
    {
        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PasswordHash = passwordHash
        };
    }
    
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
