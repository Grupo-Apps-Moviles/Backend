using System.Text.Json.Serialization;
using Backend_Frock.IAM.Domain.Model.ValueObjects;

namespace Backend_Frock.IAM.Domain.Model.Aggregates;

public partial class User(string email, string username, string passwordHash, Role role)
{
    public User() : this(string.Empty, string.Empty, string.Empty, Role.Traveller)
    {
    }

    public int Id { get; }
    public string Email { get; private set; } = email;
    public string Username { get; private set; } = username;
    public Role Role { get; private set; } = role;

    [JsonIgnore] public string PasswordHash { get; private set; } = passwordHash;

    public User UpdateEmail(string email)
    {
        Email = email;
        return this;
    }

    public User UpdateUsername(string username)
    {
        Username = username;
        return this;
    }

    public User UpdatePasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        return this;
    }
    public User UpdateRole(Role role)
    {
        Role = role;
        return this;
    }
}