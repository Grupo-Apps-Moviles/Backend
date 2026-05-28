// Companies
using Backend_Frock.Companies.Application.Internal.CommandServices;
using Backend_Frock.Companies.Application.Internal.QueryServices;
using Backend_Frock.Companies.Domain.Repositories;
using Backend_Frock.Companies.Domain.Services;
using Backend_Frock.Companies.Infrastructure.Repositories;

// User
using Backend_Frock.IAM.Application.CommandServices;
using Backend_Frock.IAM.Application.OutboundServices;
using Backend_Frock.IAM.Application.QueryServices;
using Backend_Frock.IAM.Domain.Repositories;
using Backend_Frock.IAM.Domain.Services;
using Backend_Frock.IAM.Infrastructure.Hashing.BCrypt.Services;
using Backend_Frock.IAM.Infrastructure.Persistence.EFC.Repositories;
using Backend_Frock.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using Backend_Frock.IAM.Infrastructure.Tokens.JWT.Configuration;
using Backend_Frock.IAM.Infrastructure.Tokens.JWT.Services;
using Backend_Frock.IAM.Interfaces.ACL;
using Backend_Frock.IAM.Interfaces.ACL.Services;

// Reservations
using Backend_Frock.Reservations.Application.Internal.CommandServices;
using Backend_Frock.Reservations.Application.Internal.QueryServices;
using Backend_Frock.Reservations.Domain.Repositories;
using Backend_Frock.Reservations.Domain.Services;
using Backend_Frock.Reservations.Infrastructure.Repositories;

// Routes
using Backend_Frock.Routes.Application.Internal.CommandServices;
using Backend_Frock.Routes.Application.Internal.QueryServices;
using Backend_Frock.Routes.Domain.Repository;
using Backend_Frock.Routes.Domain.Service;
using Backend_Frock.Routes.Infrastructure.Repositories;

// Shared
using Backend_Frock.Shared.Domain.Repositories;
using Backend_Frock.Shared.Domain.Services;
using Backend_Frock.Shared.Infrastructure.Configuration;
using Backend_Frock.Shared.Infrastructure.Interfaces.ASP.Configuration;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Configuration;
using Backend_Frock.Shared.Infrastructure.Persistence.EFC.Repositories;
using Backend_Frock.Shared.Infrastructure.Services;

// Stop
using Backend_Frock.Stops.Application.External;
using Backend_Frock.Stops.Application.Internal.CommandServices;
using Backend_Frock.Stops.Application.Internal.CommandServices.Geographic;
using Backend_Frock.Stops.Application.Internal.QueryServices;
using Backend_Frock.Stops.Application.Internal.QueryServices.Geographic;
using Backend_Frock.Stops.Domain.Repositories;
using Backend_Frock.Stops.Domain.Repositories.Geographic;
using Backend_Frock.Stops.Domain.Services;
using Backend_Frock.Stops.Domain.Services.Geographic;
using Backend_Frock.Stops.Infrastructure.Repositories;
using Backend_Frock.Stops.Infrastructure.Repositories.Geographic;
using Backend_Frock.Stops.Infrastructure.Seeding;

// Subscriptions
using Backend_Frock.Subscriptions.Application.Internal.CommandServices;
using Backend_Frock.Subscriptions.Application.Internal.QueryServices;
using Backend_Frock.Subscriptions.Domain.Model.Repository;
using Backend_Frock.Subscriptions.Domain.Service;
using Backend_Frock.Subscriptions.Infrastructure.Paypal;
using Backend_Frock.Subscriptions.Infrastructure.Repositories;
using Backend_Frock.Subscriptions.Infrastructure.Services;

// Microsoft
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Lower Case URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Configure Kebab Case Route Naming Convention
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Viacore Backend Develop",
            Version = "v1",
            Description = "Viacore Backend API",
            TermsOfService = new Uri("https://github.com/velardesoft/"),
            Contact = new OpenApiContact
            {
                Name = "Grupo de estudiantes de la universidad peruana de Ciencias Aplicadas del curso de Aplicaciones Para Dispositivos Móviles",
                Email = "codydevops@gmail.com"
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0",
                Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
            }
        });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

// Obtener conexión del appsettings.json o de variables de entorno de Railway
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Si estamos en Railway, construimos la cadena con variables internas para mayor velocidad
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MYSQLHOST")))
{
    var host = Environment.GetEnvironmentVariable("MYSQLHOST");
    var port = Environment.GetEnvironmentVariable("MYSQLPORT");
    var user = Environment.GetEnvironmentVariable("MYSQLUSER");
    var pass = Environment.GetEnvironmentVariable("MYSQLPASSWORD");
    var db   = Environment.GetEnvironmentVariable("MYSQLDATABASE");

    connectionString = $"Server={host};Port={port};Database={db};Uid={user};Pwd={pass};SslMode=Required;";
}

if (connectionString is null)
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString, mysqlOptions =>
    {
        mysqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    });
});

// Configure Database Context and Logging Levels
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });
else if (builder.Environment.IsProduction())
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error)
            .EnableDetailedErrors();
    });


// Configure Dependency Injection

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IAM Bounded Context Injection Configuration
// TokenSettings Configuration
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

//Company
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyCommandService, CompanyCommandService>();
builder.Services.AddScoped<ICompanyQueryService, CompanyQueryService>();

//Geographic
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IRegionCommandService, RegionCommandService>();
builder.Services.AddScoped<IRegionQueryService, RegionQueryService>();
/**/
builder.Services.AddScoped<IProvinceRepository, ProvinceRepository>();
builder.Services.AddScoped<IProvinceCommandService, ProvinceCommandService>();
builder.Services.AddScoped<IProvinceQueryService, ProvinceQueryService>();
/**/
builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
builder.Services.AddScoped<IDistrictCommandService, DistrictCommandService>();
builder.Services.AddScoped<IDistrictQueryService, DistrictQueryService>();
/**/

//Stops
builder.Services.AddScoped<IStopRepository, StopRepository>();
builder.Services.AddScoped<IStopCommandService, StopCommandService>();
builder.Services.AddScoped<IStopQueryService, StopQueryService>();

//Routes
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRouteCommandService, RouteCommandService>();
builder.Services.AddScoped<IRouteQueryService, RouteQueryService>();

// Subscriptions
builder.Services.Configure<PaypalOptions>(builder.Configuration.GetSection(PaypalOptions.SectionName));
builder.Services.AddHttpClient<PaypalService>();
builder.Services.AddScoped<IPaypalService, PaypalService>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();

//Reservations
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationCommandService, ReservationCommandService>();
builder.Services.AddScoped<IReservationQueryService, ReservationQueryService>();

//GEOSERVICE
builder.Services.AddHttpClient<IGeoImportService, GeoImportService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["GeoApi:BaseUrl"]);
});
//Seeding Service Geographic Data
// Datos iniciales fijos de datos geográficos
builder.Services.AddScoped<GeographicDataSeeder>();

// CORS - Configuración para permitir 

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // Permite cualquier puerto y cualquier IP
            .AllowAnyHeader()
            .AllowAnyMethod();
        // IMPORTANTE: No uses .AllowCredentials() aquí, o el navegador dará error de seguridad.
    });
});

// Cloudinary Configuration
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

var app = builder.Build();

app.UseCors();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    // Seed initial geographic data
    try
    {
        var seeder = services.GetRequiredService<GeographicDataSeeder>();
        await seeder.SeedDataAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error durante la carga de datos iniciales.");
    }
}


// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty; // Esto hace que Swagger sea la página principal en Railway
});

//app.UseHttpsRedirection();
app.UseRouting(); // Si no está implícito
app.UseRequestAuthorization(); // Tu middleware personalizado
app.UseAuthentication();
app.UseAuthorization(); // Authorization de ASP.NET Core
app.MapControllers();

app.Run();

