using ElectronicMedicalCert.Application.Common.Exceptions;
using ElectronicMedicalCert.Application.Common.Interfaces;
using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Common;
using ElectronicMedicalCert.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.InvalidatePosudekRo;

public sealed class InvalidatePosudekRoCommandHandler(IElectronicMedicalCertDbContext db)
    : IRequestHandler<InvalidatePosudekRoCommand, PosudekRoDetailDto>
{
    public async Task<PosudekRoDetailDto> Handle(InvalidatePosudekRoCommand request, CancellationToken cancellationToken)
    {
        var current = await db.PosudkyRo
            .AsNoTracking()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (current is null)
        {
            throw new NotFoundException("Požadovaný posudek nebyl nalezen.");
        }

        var currentEtag = Convert.ToBase64String(current.RowVersion);
        if (!string.Equals(currentEtag, request.IfMatch, StringComparison.Ordinal))
        {
            throw new ConflictException("Došlo ke konfliktu – záznam byl mezitím změněn.");
        }

        var lookup = new CodebookLookup(db);
        var zneplatneny = await lookup.ResolveItem("stavPosudku", "STAV-POSUDKU", "stav_posudku_zneplatneny", "1", cancellationToken);
        if (current.StavPosudkuPolozkaId == zneplatneny.Id)
        {
            throw new ConflictException("Došlo ke konfliktu – posudek je již zneplatněn.");
        }

        var akce = await lookup.ResolveItem("typAkce", "TYP-AKCE", "akce_ro_zneplatneni", "1", cancellationToken);
        var typOperace = await lookup.ResolveItem("typOperace", "TYP-OPERACE", "INVALIDATE", "1", cancellationToken);

        var newRowVersion = Guid.NewGuid().ToByteArray();

        // Update using a stub entity to avoid graph-tracking issues across providers.
        var stub = new PosudekRo { Id = current.Id };
        db.PosudkyRo.Attach(stub);
        stub.StavPosudkuPolozkaId = zneplatneny.Id;
        stub.TypAkcePolozkaId = akce.Id;
        stub.RowVersion = newRowVersion;

        var entry = db.PosudkyRo.Entry(stub);
        entry.Property(x => x.StavPosudkuPolozkaId).IsModified = true;
        entry.Property(x => x.TypAkcePolozkaId).IsModified = true;
        entry.Property(x => x.RowVersion).IsModified = true;

        db.PosudkyRoHistorie.Add(new PosudekRoHistorie
        {
            Id = Guid.NewGuid(),
            PosudekRoId = current.Id,
            TypOperacePolozkaId = typOperace.Id,
            DatumOperace = DateTime.UtcNow,
            KrzpIdLekare = current.KrzpId,
            IcoPoskytovatele = current.Ico,
        });

        await db.SaveChangesAsync(cancellationToken);

        var updated = await db.PosudkyRo
            .AsNoTracking()
            .Include(p => p.Zpusobilosti)
                .ThenInclude(z => z.SkupinyRidicskehoOpravneni)
            .Include(p => p.Zpusobilosti)
                .ThenInclude(z => z.HarmonizovaneKody)
                    .ThenInclude(h => h.SkupinaRo)
            .Include(p => p.Zpusobilosti)
                .ThenInclude(z => z.NarodniKody)
            .Where(p => p.Id == request.Id)
            .FirstAsync(cancellationToken);

        var usedIds = new List<Guid>
        {
            updated.StavPosudkuPolozkaId,
            updated.DruhProhlidkyPolozkaId,
            updated.DruhPosudkuPolozkaId,
        };
        usedIds.AddRange(updated.Zpusobilosti.Select(z => z.SkupinaZadateleRidicPolozkaId));
        usedIds.AddRange(updated.Zpusobilosti.Select(z => z.VysledekPolozkaId));
        usedIds.AddRange(updated.Zpusobilosti.SelectMany(z => z.SkupinyRidicskehoOpravneni.Select(s => s.SkupinaRoPolozkaId)));
        usedIds.AddRange(updated.Zpusobilosti.SelectMany(z => z.HarmonizovaneKody.Select(h => h.HarmonizovanyKodPolozkaId)));
        usedIds.AddRange(updated.Zpusobilosti.SelectMany(z => z.HarmonizovaneKody.SelectMany(h => h.SkupinaRo.Select(sr => sr.SkupinaRoPolozkaId))));
        usedIds.AddRange(updated.Zpusobilosti.SelectMany(z => z.NarodniKody.Select(n => n.NarodniKodPolozkaId)));
        usedIds.AddRange(updated.Zpusobilosti.SelectMany(z => z.NarodniKody.Select(n => n.SkupinaRoPolozkaId)));

        var itemsById = await lookup.LoadItemsByIds(usedIds, cancellationToken);
        return PosudekRoDtoFactory.ToDetail(updated, itemsById);
    }
}

