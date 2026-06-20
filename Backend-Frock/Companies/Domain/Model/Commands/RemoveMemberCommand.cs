namespace Backend_Frock.Companies.Domain.Model.Commands
{
    public record RemoveMemberCommand(int RequesterUserId, int TargetMembershipId);
}
