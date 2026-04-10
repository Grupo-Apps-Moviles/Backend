using Backend_Frock.IAM.Domain.Model.Commands;
using Backend_Frock.IAM.Interfaces.REST.Resources;

namespace Backend_Frock.IAM.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Email, resource.Password);
    }
}