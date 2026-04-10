using ElectronicMedicalCert.Application.Common.Interfaces;
using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Common;
using ElectronicMedicalCert.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CreatePosudekRo;

public sealed class CreatePosudekRoCommandHandler(IElectronicMedicalCertDbContext db)
    : IRequestHandler<CreatePosudekRoCommand, PosudekRoCreateResultDto>
{
    public async Task<PosudekRoCreateResultDto> Handle(CreatePosudekRoCommand request, CancellationToken cancellationToken)
    {
        var lookup = new CodebookLookup(db);

        var typAkce = await lookup.ResolveItem(nameof(request.Dto.TypAkce), "TYP-AKCE", request.Dto.TypAkce.Kod, request.Dto.TypAkce.Verze, cancellationToken);
        var stavPosudku = await lookup.ResolveItem(nameof(request.Dto.StavPosudku), "STAV-POSUDKU", request.Dto.StavPosudku.Kod, request.Dto.StavPosudku.Verze, cancellationToken);
        var druhProhlidky = await lookup.ResolveItem(nameof(request.Dto.DruhProhlidky), "DRUH-PROHLIDKY", request.Dto.DruhProhlidky.Kod, request.Dto.DruhProhlidky.Verze, cancellationToken);
        var druhPosudku = await lookup.ResolveItem(nameof(request.Dto.DruhPosudku), "DRUH-POSUDKU", request.Dto.DruhPosudku.Kod, request.Dto.DruhPosudku.Verze, cancellationToken);

        var worker = await db.AuthorizedWorkers.AsNoTracking()
            .Where(x => x.KrzpId == request.Dto.KrzpId)
            .OrderBy(x => x.Ico)
            .FirstOrDefaultAsync(cancellationToken);

        var posudek = new PosudekRo
        {
            Id = Guid.NewGuid(),
            Rid = request.Dto.Rid,
            KrzpId = request.Dto.KrzpId,
            Ico = worker?.Ico ?? "12345678",
            DatumVystaveni = request.Dto.DatumVystaveni,
            PlatnostDo = request.Dto.PlatnostDo,
            DatumVytvoreni = DateTime.UtcNow,
            OpakovanyPosudekId = request.Dto.OpakovanyPosudekId,
            TypAkcePolozkaId = typAkce.Id,
            StavPosudkuPolozkaId = stavPosudku.Id,
            DruhProhlidkyPolozkaId = druhProhlidky.Id,
            DruhPosudkuPolozkaId = druhPosudku.Id,
        };

        foreach (var zpDto in request.Dto.Zpusobilosti)
        {
            var skupinaZadatele = await lookup.ResolveItem(nameof(zpDto.SkupinaZadateleRidic), "SKUPINA-ZADATEL-RIDIC", zpDto.SkupinaZadateleRidic.Kod, zpDto.SkupinaZadateleRidic.Verze, cancellationToken);
            var vysledek = await lookup.ResolveItem(nameof(zpDto.Vysledek), "VYSLEDEK", zpDto.Vysledek.Kod, zpDto.Vysledek.Verze, cancellationToken);

            var zp = new PosudekRoZpusobilost
            {
                Id = Guid.NewGuid(),
                SkupinaZadateleRidicPolozkaId = skupinaZadatele.Id,
                VysledekPolozkaId = vysledek.Id,
            };

            foreach (var g in zpDto.SkupinyRidicskehoOpravneni)
            {
                var skupinaRo = await lookup.ResolveItem(nameof(g.SkupinaRo), "SKUPINA-RO", g.SkupinaRo.Kod, g.SkupinaRo.Verze, cancellationToken);
                zp.SkupinyRidicskehoOpravneni.Add(new PosudekRoSkupina
                {
                    Id = Guid.NewGuid(),
                    SkupinaRoPolozkaId = skupinaRo.Id,
                });
            }

            foreach (var h in zpDto.HarmonizovaneKody ?? Enumerable.Empty<PosudekRoHarmonizovanyKodCreateDto>())
            {
                var hk = await lookup.ResolveItem(nameof(h.HarmonizovanyKod), "HARMON-KOD", h.HarmonizovanyKod.Kod, h.HarmonizovanyKod.Verze, cancellationToken);
                var ent = new PosudekRoHarmonizovanyKod
                {
                    Id = Guid.NewGuid(),
                    HarmonizovanyKodPolozkaId = hk.Id,
                    UpresneniText = h.UpresneniText,
                };

                foreach (var sr in h.SkupinaRo ?? new List<PosudekRoCiselnikPolozkaCreateDto>())
                {
                    var skupinaRo = await lookup.ResolveItem(nameof(h.SkupinaRo), "SKUPINA-RO", sr.Kod, sr.Verze, cancellationToken);
                    ent.SkupinaRo.Add(new PosudekRoHarmonizovanyKodSkupinaRo
                    {
                        Id = Guid.NewGuid(),
                        SkupinaRoPolozkaId = skupinaRo.Id,
                    });
                }

                zp.HarmonizovaneKody.Add(ent);
            }

            foreach (var n in zpDto.NarodniKody ?? Enumerable.Empty<PosudekRoNarodniKodCreateDto>())
            {
                var nk = await lookup.ResolveItem(nameof(n.NarodniKod), "NAROD-KOD", n.NarodniKod.Kod, n.NarodniKod.Verze, cancellationToken);
                var skupinaRo = await lookup.ResolveItem(nameof(n.SkupinaRo), "SKUPINA-RO", n.SkupinaRo.Kod, n.SkupinaRo.Verze, cancellationToken);
                zp.NarodniKody.Add(new PosudekRoNarodniKod
                {
                    Id = Guid.NewGuid(),
                    NarodniKodPolozkaId = nk.Id,
                    SkupinaRoPolozkaId = skupinaRo.Id,
                    UpresneniText = n.UpresneniText,
                });
            }

            posudek.Zpusobilosti.Add(zp);
        }

        posudek.Historie.Add(new PosudekRoHistorie
        {
            Id = Guid.NewGuid(),
            TypOperacePolozkaId = (await lookup.ResolveItem("typOperace", "TYP-OPERACE", "CREATE", "1", cancellationToken)).Id,
            DatumOperace = DateTime.UtcNow,
            KrzpIdLekare = posudek.KrzpId,
            IcoPoskytovatele = posudek.Ico,
        });

        db.PosudkyRo.Add(posudek);
        await db.SaveChangesAsync(cancellationToken);

        // Preload all codebook items referenced by the created aggregate for DTO projection.
        var usedIds = new List<Guid>
        {
            posudek.StavPosudkuPolozkaId,
            posudek.DruhProhlidkyPolozkaId,
            posudek.DruhPosudkuPolozkaId,
        };
        usedIds.AddRange(posudek.Zpusobilosti.Select(z => z.SkupinaZadateleRidicPolozkaId));
        usedIds.AddRange(posudek.Zpusobilosti.Select(z => z.VysledekPolozkaId));
        usedIds.AddRange(posudek.Zpusobilosti.SelectMany(z => z.SkupinyRidicskehoOpravneni.Select(s => s.SkupinaRoPolozkaId)));
        usedIds.AddRange(posudek.Zpusobilosti.SelectMany(z => z.HarmonizovaneKody.Select(h => h.HarmonizovanyKodPolozkaId)));
        usedIds.AddRange(posudek.Zpusobilosti.SelectMany(z => z.HarmonizovaneKody.SelectMany(h => h.SkupinaRo.Select(sr => sr.SkupinaRoPolozkaId))));
        usedIds.AddRange(posudek.Zpusobilosti.SelectMany(z => z.NarodniKody.Select(n => n.NarodniKodPolozkaId)));
        usedIds.AddRange(posudek.Zpusobilosti.SelectMany(z => z.NarodniKody.Select(n => n.SkupinaRoPolozkaId)));

        var itemsById = await lookup.LoadItemsByIds(usedIds, cancellationToken);

        return PosudekRoDtoFactory.ToCreateResult(posudek, itemsById);
    }
}

