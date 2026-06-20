namespace Backend_Frock.Companies.Domain.Model.Commands
{
    public record RegenerateInvitationCodeCommand(int RequesterUserId, int CompanyId);
}
