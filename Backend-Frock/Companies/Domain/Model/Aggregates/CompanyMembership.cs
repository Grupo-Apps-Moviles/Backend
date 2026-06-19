using Backend_Frock.Companies.Domain.Model.ValueObjects;

namespace Backend_Frock.Companies.Domain.Model.Aggregates;

public class CompanyMembership
{
    public int Id { get; }
    public int CompanyId { get; private set; }
    public int UserId { get; private set; }            // ref a IAM.User por id (sin acoplar)
    public MemberRole MemberRole { get; private set; }
    public DateTime JoinedAt { get; private set; }

    protected CompanyMembership() { }                  // EF

    public CompanyMembership(int companyId, int userId, MemberRole role)
    {
        CompanyId  = companyId;
        UserId     = userId;
        MemberRole = role;
        JoinedAt   = DateTime.UtcNow;
    }

    public bool IsAdmin => MemberRole == MemberRole.Admin;

    public void PromoteToAdmin() => MemberRole = MemberRole.Admin;
    public void DemoteToDriver() => MemberRole = MemberRole.Driver;
}
