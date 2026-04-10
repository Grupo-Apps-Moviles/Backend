using Backend_Frock.IAM.Domain.Model.ValueObjects;

namespace Backend_Frock.IAM.Interfaces.REST.Resources;

public class SignUpResource
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Role Role { get; set; }
}