using Backend_Frock.IAM.Domain.Model.Aggregates;
using Backend_Frock.IAM.Interfaces.REST.Resources;

namespace Backend_Frock.IAM.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(
        User user, string token)
    {
        return new AuthenticatedUserResource(user.Id, user.Username, user.Role, token);
    }
}