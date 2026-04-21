using ElectronicMedicalCert.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Tests.Unit;

internal static class TestDbFactory
{
    public static async Task<ElectronicMedicalCertDbContext> CreateSeededAsync()
    {
        var opts = new DbContextOptionsBuilder<ElectronicMedicalCertDbContext>()
            .UseInMemoryDatabase($"UnitDb_{Guid.NewGuid():N}")
            .EnableSensitiveDataLogging()
            .Options;

        var db = new ElectronicMedicalCertDbContext(opts);
        var loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder => { });
        var logger = loggerFactory.CreateLogger("TestDbFactory");
        await DatabaseSeeder.SeedAsync(db, logger);
        return db;
    }
}

