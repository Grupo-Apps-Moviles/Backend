using Microsoft.AspNetCore.Http;

namespace Backend_Frock.Companies.Interfaces.REST.Resources
{
    public class CreateCompanyWithFileResource
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile? LogoFile { get; set; }
        public int FkIdUser { get; set; }
    }
}