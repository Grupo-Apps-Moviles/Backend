using Backend_Frock.Companies.Domain.Model.Aggregates;
using Backend_Frock.Favorites.Domain.Model.Aggregates;
using Backend_Frock.IAM.Domain.Model.Aggregates;
using Backend_Frock.Reservations.Domain.Model.Aggregates;
using Backend_Frock.Routes.Domain.Model.Aggregates;
using Backend_Frock.Routes.Domain.Model.Entities;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Backend_Frock.Stops.Domain.Model.Aggregates;
using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Subscriptions.Domain.Model.Aggregates;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context for the Learning Center Platform
/// </summary>
/// <param name="options">
///     The options for the database context
/// </param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyMembership> CompanyMemberships { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<RouteAggregate> Routes { get; set; }
    public DbSet<RoutesStops> RouteStops { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // IAM Context
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(u => u.Username).IsRequired();
        builder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
        builder.Entity<User>().Property(u => u.Role).HasConversion<string>().IsRequired();

        //COMPANY
        builder.Entity<Company>().HasKey(f => f.Id);
        builder.Entity<Company>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Company>().Property(f => f.Name).IsRequired();
        builder.Entity<Company>().Property(f => f.LogoUrl).IsRequired();
        // Fix for CS1660: The issue arises because the lambda expression is being used in a context where a string is expected.
        // The correct fix is to specify the type explicitly for the foreign key property.
        builder.Entity<Company>()
            .HasOne<User>() // Una Company tiene un User (dueño)
            .WithOne() // Un User puede tener una Company (dueño)
            .HasForeignKey<Company>(c => c.FkIdUser) // Specify the entity type explicitly
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        // Invitation code (NUEVO) - único por compañía
        builder.Entity<Company>().Property(c => c.InvitationCode).HasMaxLength(12);
        builder.Entity<Company>().HasIndex(c => c.InvitationCode).IsUnique();

        // COMPANY MEMBERSHIP
        builder.Entity<CompanyMembership>().HasKey(m => m.Id);
        builder.Entity<CompanyMembership>().Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<CompanyMembership>().Property(m => m.CompanyId).IsRequired();
        builder.Entity<CompanyMembership>().Property(m => m.UserId).IsRequired();
        builder.Entity<CompanyMembership>().Property(m => m.MemberRole).IsRequired();
        builder.Entity<CompanyMembership>().Property(m => m.JoinedAt).IsRequired();
        // Un usuario solo puede tener una membresía
        builder.Entity<CompanyMembership>().HasIndex(m => m.UserId).IsUnique();
        // Evitar duplicado company+user
        builder.Entity<CompanyMembership>().HasIndex(m => new { m.CompanyId, m.UserId }).IsUnique();

        //REGION
        builder.Entity<Region>().HasKey(f => f.Id);
        builder.Entity<Region>().Property(f => f.Id)
            .ValueGeneratedNever() // Id estático provisto por el seeder (CODIGO), no autogenerado por la BD.
            .IsRequired();
        builder.Entity<Region>().Property(f => f.Name).IsRequired();

        //PROVINCE
        builder.Entity<Province>().HasKey(f => f.Id);
        builder.Entity<Province>().Property(f => f.Id)
            .ValueGeneratedNever() // Id estático provisto por el seeder (CODIGO), no autogenerado por la BD.
            .IsRequired();
        builder.Entity<Province>().Property(f => f.Name).IsRequired();
        builder.Entity<Province>()
            .HasOne<Region>()
            .WithMany()
            .HasForeignKey(p => p.FkIdRegion)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        //DISTRICT
        builder.Entity<District>().HasKey(f => f.Id);
        builder.Entity<District>().Property(f => f.Id)
            .ValueGeneratedNever() // Id estático provisto por el seeder (CODIGO), no autogenerado por la BD.
            .IsRequired();
        builder.Entity<District>().Property(f => f.Name).IsRequired();
        builder.Entity<District>()
            .HasOne<Province>()
            .WithMany()
            .HasForeignKey(d => d.FkIdProvince)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        //STOP
        builder.Entity<Stop>().HasKey(f => f.Id);
        builder.Entity<Stop>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Stop>().Property(f => f.Name).IsRequired();
        builder.Entity<Stop>().Property(f => f.GoogleMapsUrl).IsRequired();
        builder.Entity<Stop>().Property(f => f.ImageUrl).IsRequired();
        builder.Entity<Stop>().Property(f => f.Phone).IsRequired();
        builder.Entity<Stop>().Property(f => f.Address).IsRequired();
        builder.Entity<Stop>().Property(f => f.Reference).IsRequired();

        builder.Entity<Stop>()
            .HasOne<Company>() // Un Stop tiene una Company
            .WithMany() // Una Company tiene muchos Stops
            .HasForeignKey(l => l.FkIdCompany)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Stop>()
            .HasOne<District>() // Un Stop tiene una District
            .WithMany() // Una District tiene muchos Stops
            .HasForeignKey(f => f.FkIdDistrict)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // SUBSCRIPTION
        builder.Entity<Subscription>(b =>
        {
            b.ToTable("subscriptions");
            b.HasKey(s => s.Id);
            b.Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
            b.Property(s => s.CompanyId).IsRequired();
            b.Property(s => s.PaypalSubscriptionId).HasMaxLength(100).IsRequired();
            b.Property(s => s.PaypalPlanId).HasMaxLength(100).IsRequired();
            b.Property(s => s.Status).HasConversion<string>().IsRequired();
            b.Property(s => s.MaxMembers).IsRequired();
            b.Property(s => s.StartDate).IsRequired();
            b.Property(s => s.EndDate).IsRequired();
            b.Property(s => s.CreatedAt).IsRequired();
            b.Property(s => s.UpdatedAt).IsRequired();
            b.Ignore(s => s.IsActive); // computed, no va a la DB
        });
        
        // ROUTE
        builder.Entity<RouteAggregate>(b =>
            {
                b.ToTable("Routes");
                b.HasKey(r => r.Id);
                b.Property(r => r.Price).IsRequired();
                b.Property(r => r.Duration).IsRequired();
                b.Property(r => r.Frequency).IsRequired();
                // 2) Schedule como entidad hija (1-N)
                b.HasMany(r => r.Schedules)
                    .WithOne() // si no navegas hacia RouteAggregate
                    .HasForeignKey(s => s.RouteId)
                    .OnDelete(DeleteBehavior.Cascade);
                // Owned type: RouteStop → tabla intermedia
                // RouteStops (join table)
                builder.Entity<RoutesStops>(b =>
                {
                    b.ToTable("RouteStops");
                    // clave compuesta RouteId+StopId
                    b.HasKey(rs => rs.Id);

                    // relación a RouteAggregate
                    b.HasOne(rs => rs.Route)
                        .WithMany(r => r.Stops)
                        .HasForeignKey(rs => rs.FKRouteId);

                    // relación a Stop
                    b.HasOne(rs => rs.Stop)
                        .WithMany()
                        .HasForeignKey(rs => rs.FkStopId)
                        .OnDelete(DeleteBehavior.Restrict);
                });
            }
        );
        // 4) Schedule
        builder.Entity<Schedule>(b =>
        {
            b.ToTable("Schedules");
            b.HasKey(s => s.Id);
            b.Property(s => s.StartTime).IsRequired();
            b.Property(s => s.EndTime).IsRequired();
            b.Property(s => s.DayOfWeek).HasMaxLength(10);
        });
        
        // Reservas
        builder.Entity<Reservation>().ToTable("Reservations");
        builder.Entity<Reservation>().HasKey(r => r.Id);
        builder.Entity<Reservation>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Reservation>().Property(r => r.Status).IsRequired().HasConversion<string>();

        builder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);
        
        builder.Entity<Reservation>()
            .HasIndex(r => r.PaypalTransactionId)
            .IsUnique();
        
        builder.Entity<ReservationRoute>().ToTable("ReservationRoutes");
        builder.Entity<ReservationRoute>().HasKey(rr => new { rr.ReservationId, rr.RouteId });

        builder.Entity<ReservationRoute>()
            .HasOne(rr => rr.Reservation)
            .WithMany(r => r.ReservationRoutes)
            .HasForeignKey(rr => rr.ReservationId);

        builder.Entity<ReservationRoute>()
            .HasOne(rr => rr.Routes)
            .WithMany()
            .HasForeignKey(rr => rr.RouteId);
        
        // Favorites
        builder.Entity<FavoriteRoute>().ToTable("FavoriteRoutes");
        builder.Entity<FavoriteRoute>().HasKey(f => f.Id);
        builder.Entity<FavoriteRoute>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<FavoriteRoute>().Property(f => f.PassengerId).IsRequired();
        builder.Entity<FavoriteRoute>().Property(f => f.RouteId).IsRequired();
        builder.Entity<FavoriteRoute>().Property(f => f.CreatedAt).IsRequired();
        builder.Entity<FavoriteRoute>().HasIndex(f => new { f.PassengerId, f.RouteId }).IsUnique();

        builder.UseSnakeCaseNamingConvention();
    }
}