using Backend_Frock.Companies.Domain.Model.Commands;

namespace Backend_Frock.Companies.Domain.Model.Aggregates
{
    public class Company
    {
        public int Id { get; }
        public string Name { get; set; }
        public string? LogoUrl { get; set; }
        public int FkIdUser { get; set; }                  // creador (legacy)
        public string InvitationCode { get; private set; } = string.Empty;  // NUEVO

        protected Company()
        {
            Name = string.Empty;
            LogoUrl = string.Empty;
            FkIdUser = 0;
        }

        public Company(CreateCompanyCommand command)
        {
            Name = command.Name;
            LogoUrl = command.LogoUrl;
            FkIdUser = command.FkIdUser;
            InvitationCode = GenerateInvitationCode();     // NUEVO
        }

        public Company(UpdateCompanyCommand command)
        {
            Id = command.Id;
            Name = command.Name;
            LogoUrl = command.LogoUrl;
            FkIdUser = command.FkIdUser;
        }

        public Company(DeleteCompanyCommand command)
        {
            Id = command.Id;
            Name = "";
            LogoUrl = "";
            FkIdUser = 0;
        }

        // NUEVO
        public void RegenerateInvitationCode() => InvitationCode = GenerateInvitationCode();

        private static string GenerateInvitationCode()
        {
            // 8 chars, sin caracteres ambiguos (0/O, 1/I/L)
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
            var rng = Random.Shared;
            return new string(Enumerable.Range(0, 8)
                .Select(_ => chars[rng.Next(chars.Length)]).ToArray());
        }
    }
}
