namespace Backend_Frock.Companies.Domain.Model.Commands
{
    public record CreateCompanyCommand(
        string Name, // The name of the company
        string LogoUrl, // The URL to the company's logo image
        int FkIdUser // This is a foreign key to a User entity, indicating the user who created the company
        );
}
