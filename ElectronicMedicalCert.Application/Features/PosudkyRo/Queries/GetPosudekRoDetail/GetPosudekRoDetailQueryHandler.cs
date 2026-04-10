using ElectronicMedicalCert.Application.Common.Exceptions;
using ElectronicMedicalCert.Application.Common.Interfaces;
using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoDetail;

public sealed class GetPosudekRoDetailQueryHandler(IElectronicMedicalCertDbContext db)
    : IRequestHandler<GetPosudekRoDetailQuery, PosudekRoDetailDto>
{
    public async Task<PosudekRoDetailDto> Handle(GetPosudekRoDetailQuery request, CancellationToken cancellationToken)
    {
        var posudek = await db.PosudkyRo
            .AsNoTracking()
            .Include(p => p.Zpusobilosti)
                .ThenInclude(z => z.SkupinyRidicskehoOpravneni)
            .Include(p => p.Zpusobilosti)
                .ThenInclude(z => z.HarmonizovaneKody)
                    .ThenInclude(h => h.SkupinaRo)
            .Include(p => p.Zpusobilosti)
                .ThenInclude(z => z.NarodniKody)
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (posudek is null)
        {
            throw new NotFoundException("Požadovaný posudek nebyl nalezen.");
        }

        var lookup = new CodebookLookup(db);
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
        return PosudekRoDtoFactory.ToDetail(posudek, itemsById);
    }
}

