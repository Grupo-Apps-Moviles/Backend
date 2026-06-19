using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Companies.Interfaces.REST.Resources;

namespace Backend_Frock.Companies.Interfaces.REST.Transform
{
    public static class MembershipResourceFromEntityAssembler
    {
        public static MembershipResource ToResourceFromEntity(CompanyMembership membership, Company company) =>
            new MembershipResource(
                membership.Id,
                membership.CompanyId,
                company.Name,
                membership.UserId,
                membership.MemberRole.ToString(),
                membership.JoinedAt
            );

        public static MyMembershipResource ToMyResourceFromEntity(CompanyMembership membership, Company company) =>
            new MyMembershipResource(
                membership.CompanyId,
                company.Name,
                company.LogoUrl,
                membership.MemberRole.ToString(),
                membership.IsAdmin ? company.InvitationCode : null
            );
    }
}
