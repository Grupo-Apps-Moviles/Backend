namespace Backend_Frock.Companies.Domain.Model.Commands
{
    public record JoinCompanyCommand(int UserId, string InvitationCode);
}
