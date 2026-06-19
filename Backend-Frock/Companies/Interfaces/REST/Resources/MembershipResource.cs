namespace Backend_Frock.Companies.Interfaces.REST.Resources
{
    public record MembershipResource(
        int Id, // The unique identifier for the membership
        int CompanyId, // The company the member belongs to
        string CompanyName, // The name of the company
        int UserId, // The IAM user behind this membership
        string MemberRole, // Admin or Driver
        DateTime JoinedAt // When the user joined the company
        );
}
