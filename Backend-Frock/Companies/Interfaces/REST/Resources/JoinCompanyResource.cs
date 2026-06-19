namespace Backend_Frock.Companies.Interfaces.REST.Resources
{
    public record JoinCompanyResource(
        string InvitationCode // The invitation code shared by the company admin
        );
}
