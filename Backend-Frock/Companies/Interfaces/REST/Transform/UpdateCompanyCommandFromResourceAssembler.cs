using Backend_Frock.Companies.Domain.Model.Commands;
using Backend_Frock.Companies.Interfaces.REST.Resources;


namespace Backend_Frock.Companies.Interfaces.REST.Transform
{
    public class UpdateCompanyCommandFromResourceAssembler
    {
        public static UpdateCompanyCommand ToCommandFromResource(UpdateCompanyResource resource)
        {
            return new UpdateCompanyCommand(
                resource.Id,
                resource.Name,
                resource.LogoUrl,
                resource.FkIdUser
            );
        }
    }
}
