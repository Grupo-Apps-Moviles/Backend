using Backend_Frock.IAM.Domain.Model.ValueObjects;

namespace Backend_Frock.IAM.Interfaces.REST.Resources;

public record AuthenticatedUserResource(int Id, string Username, Role Role, string Token);