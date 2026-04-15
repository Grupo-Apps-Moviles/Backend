using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Backend_Frock.Companies.Interfaces.REST.Resources
{
    public class CreateCompanyFormResource
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int FkIdUser { get; set; }
        
        public IFormFile? LogoFile { get; set; }
    }
}