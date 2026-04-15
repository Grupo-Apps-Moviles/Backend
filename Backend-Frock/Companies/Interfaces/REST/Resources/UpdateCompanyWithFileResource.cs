using Microsoft.AspNetCore.Http;

namespace Backend_Frock.Companies.Interfaces.REST.Resources
{
    public class UpdateCompanyWithFileResource
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; } // URL actual del logo
        public IFormFile? LogoFile { get; set; } // Nuevo archivo de logo (opcional)
        public int FkIdUser { get; set; }
    }
}