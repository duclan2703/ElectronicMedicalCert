using System.Globalization;
using System.Text;
using ElectronicMedicalCert.Application;
using ElectronicMedicalCert.Infrastructure;
using ElectronicMedicalCert.Infrastructure.Data;
using ElectronicMedicalCert.Filters;
using ElectronicMedicalCert.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, services, cfg) =>
{
    cfg
        .ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    options.Filters.Add<ModelStateValidationFilter>();
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var jwtKey = builder.Configuration["Auth:SigningKey"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        if (builder.Environment.IsEnvironment("Testing"))
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = false,
                SignatureValidator = (token, _) => new JsonWebToken(token),
            };
            return;
        }

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException("JWT signing key not configured. Set configuration 'Auth:SigningKey' or environment variable 'Auth__SigningKey'.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // consider enabling and configuring when applicable
            ValidateAudience = false, // consider enabling and configuring when applicable
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Elektronicke posudky API",
        Version = "v2.0.5",
    });

    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Zadejte `Assertion Token (JWT)`",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(ElectronicMedicalCert.Application.DependencyInjection).Assembly);

var app = builder.Build();

app.UsePathBase("/elektronickePosudky");

app.UseSerilogRequestLogging();

var supportedCultures = new[] { "cs", "en" }
    .Select(c => new CultureInfo(c))
    .ToArray();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("cs"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    ApplyCurrentCultureToResponseHeaders = true,
});

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ProblemDetailsMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = scope.ServiceProvider.GetRequiredService<ElectronicMedicalCertDbContext>();

    var autoMigrate = builder.Configuration.GetValue<bool>("Database:AutoMigrate");
    var allowAutoMigrate = app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Testing") || autoMigrate;

    try
    {
        if (db.Database.ProviderName?.Contains("SqlServer", StringComparison.OrdinalIgnoreCase) == true)
        {
            if (allowAutoMigrate)
            {
                logger.LogInformation("Applying pending EF Core migrations (AutoMigrate = {AutoMigrate})...", autoMigrate);
                db.Database.Migrate();
            }
            else
            {
                var pending = db.Database.GetPendingMigrations().ToList();
                if (pending.Any())
                {
                    logger.LogWarning("Pending EF Core migrations detected: {Pending}. Auto-migrate is disabled.", string.Join(", ", pending));

                    if (app.Environment.IsProduction())
                    {
                        throw new InvalidOperationException("Pending EF Core migrations detected. Run migrations manually or enable Database:AutoMigrate.");
                    }
                }
            }
        }
        else
        {
            logger.LogInformation("Database provider is {Provider}. Ensuring database is created.", db.Database.ProviderName);
            db.Database.EnsureCreated();
        }

        await DatabaseSeeder.SeedAsync(db);
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Database initialization failed");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/elektronickePosudky/swagger/v2/swagger.json", "Elektronicke posudky API v2");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
