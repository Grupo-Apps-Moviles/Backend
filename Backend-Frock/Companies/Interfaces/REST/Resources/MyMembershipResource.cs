namespace Backend_Frock.Companies.Interfaces.REST.Resources
{
    public record MyMembershipResource(
        int CompanyId, // The company the current user belongs to
        string CompanyName, // The name of the company
        string? LogoUrl, // The company's logo image
        string MemberRole, // Admin or Driver
        string? InvitationCode // Only filled when the requester is an Admin; otherwise null
        );
}
