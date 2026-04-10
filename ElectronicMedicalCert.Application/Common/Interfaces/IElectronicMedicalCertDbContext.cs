using ElectronicMedicalCert.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Common.Interfaces;

public interface IElectronicMedicalCertDbContext
{
    DbSet<Codebook> Codebooks { get; }
    DbSet<CodebookItem> CodebookItems { get; }
    DbSet<AuthorizedWorker> AuthorizedWorkers { get; }

    DbSet<PosudekRo> PosudkyRo { get; }
    DbSet<PosudekRoZpusobilost> PosudkyRoZpusobilosti { get; }
    DbSet<PosudekRoHistorie> PosudkyRoHistorie { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

