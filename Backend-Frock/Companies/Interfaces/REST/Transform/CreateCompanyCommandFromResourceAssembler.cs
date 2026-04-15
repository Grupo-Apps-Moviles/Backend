using Backend_Frock.Companies.Domain.Model.Commands;
using Backend_Frock.Companies.Interfaces.REST.Resources;


namespace Backend_Frock.Companies.Interfaces.REST.Transform
{
    public class CreateCompanyCommandFromResourceAssembler
    {
        public static CreateCompanyCommand ToCommandFromResource(CreateCompanyResource resource) =>
            new CreateCompanyCommand(
                resource.Name,
                resource.LogoUrl,
                resource.FkIdUser
            );
    }
}
