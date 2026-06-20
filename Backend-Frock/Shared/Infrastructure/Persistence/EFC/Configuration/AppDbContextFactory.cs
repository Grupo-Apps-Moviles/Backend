using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Design-time factory used exclusively by the EF Core tools
///     (e.g. <c>dotnet ef migrations add</c>). It is never used at runtime.
///     It lets the tools build the model without booting the whole web host
///     (which would run <c>EnsureCreated()</c>/seeding against the real database).
///     <c>migrations add</c> does not connect to the database, so the connection
///     string below is only a placeholder for the MySQL provider.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=frock;Uid=root;Pwd=root;");
        return new AppDbContext(optionsBuilder.Options);
    }
}
