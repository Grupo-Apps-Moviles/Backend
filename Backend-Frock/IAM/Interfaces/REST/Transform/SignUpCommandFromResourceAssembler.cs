using Backend_Frock.IAM.Domain.Model.Commands;
using Backend_Frock.IAM.Interfaces.REST.Resources;

namespace Backend_Frock.IAM.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand
        {
            Email = resource.Email,
            Username = resource.Username,
            Password = resource.Password,
            Role = resource.Role 
        };
    }
}