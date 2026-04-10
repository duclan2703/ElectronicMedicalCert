using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ElectronicMedicalCert.Application.Common.Interfaces;
using ElectronicMedicalCert.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace ElectronicMedicalCert.Tests.Integration;

public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = $"ElectronicMedicalCert_Test_{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ElectronicMedicalCertDbContext>>();
            services.RemoveAll<ElectronicMedicalCertDbContext>();
            services.RemoveAll<IElectronicMedicalCertDbContext>();

            services.AddDbContext<ElectronicMedicalCertDbContext>(o => o.UseInMemoryDatabase(_dbName));
            services.AddScoped<IElectronicMedicalCertDbContext>(sp => sp.GetRequiredService<ElectronicMedicalCertDbContext>());
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }

    public static AuthenticationHeaderValue CreateAuthHeader()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ZGV2LW9ubHktc2lnbmluZy1rZXktY2hhbmdlLW1lLTMyYnl0ZXMhIQ=="));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: new[] { new Claim("sub", "test-user") },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new AuthenticationHeaderValue("Bearer", jwt);
    }
}

