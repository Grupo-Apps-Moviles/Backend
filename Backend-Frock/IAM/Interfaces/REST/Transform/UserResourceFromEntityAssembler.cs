using Backend_Frock.IAM.Domain.Model.Aggregates;
using Backend_Frock.IAM.Interfaces.REST.Resources;

namespace Backend_Frock.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user)
    {
        return new UserResource(user.Id, user.Username, user.Role);
    }
}