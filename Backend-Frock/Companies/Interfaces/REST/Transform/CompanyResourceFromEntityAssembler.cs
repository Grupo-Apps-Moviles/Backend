using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Interfaces.REST.Resources;


namespace Backend_Frock.Companies.Interfaces.REST.Transform
{
    public static class CompanyResourceFromEntityAssembler
    {
        public static CompanyResource ToResourceFromEntity(Company entity) =>
            new CompanyResource(
                entity.Id,
                entity.Name,
                entity.LogoUrl,
                entity.FkIdUser
            );
    }
}
