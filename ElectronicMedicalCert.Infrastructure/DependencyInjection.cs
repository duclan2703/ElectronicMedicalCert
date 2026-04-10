using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ElectronicMedicalCert.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ElectronicMedicalCert.Application.Common.Interfaces;

namespace ElectronicMedicalCert.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ElectronicMedicalCertDbContext>(options =>
        {
            var cs = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(cs))
            {
                cs = "Server=(localdb)\\MSSQLLocalDB;Database=ElectronicMedicalCert;Trusted_Connection=True;TrustServerCertificate=True";
            }

            options.UseSqlServer(cs);
        });

        services.AddScoped<IElectronicMedicalCertDbContext>(sp => sp.GetRequiredService<ElectronicMedicalCertDbContext>());

        return services;
    }
}

