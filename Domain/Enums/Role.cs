namespace Domain.Enums;

[Flags]
public enum Role
{
    None = 0,
    Guest = 1,
    Host = 2,
    Admin = 4
}
