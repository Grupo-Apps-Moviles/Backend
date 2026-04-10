using Backend_Frock.IAM.Domain.Model.ValueObjects;

namespace Backend_Frock.IAM.Interfaces.REST.Resources;

public record UserResource(int Id, string Username, Role Role);