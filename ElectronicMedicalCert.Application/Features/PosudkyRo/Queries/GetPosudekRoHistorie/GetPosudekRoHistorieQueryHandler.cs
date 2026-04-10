using ElectronicMedicalCert.Application.Common.Exceptions;
using ElectronicMedicalCert.Application.Common.Interfaces;
using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoHistorie;

public sealed class GetPosudekRoHistorieQueryHandler(IElectronicMedicalCertDbContext db)
    : IRequestHandler<GetPosudekRoHistorieQuery, IReadOnlyList<PosudekRoHistorieDetailDto>>
{
    public async Task<IReadOnlyList<PosudekRoHistorieDetailDto>> Handle(GetPosudekRoHistorieQuery request, CancellationToken cancellationToken)
    {
        var posudek = await db.PosudkyRo
            .AsNoTracking()
            .Include(p => p.Historie)
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (posudek is null)
        {
            throw new NotFoundException("Požadovaný posudek nebyl nalezen.");
        }

        var lookup = new CodebookLookup(db);
        var ids = posudek.Historie.Select(h => h.TypOperacePolozkaId).Distinct().ToList();
        var itemsById = await lookup.LoadItemsByIds(ids, cancellationToken);

        return posudek.Historie
            .OrderByDescending(h => h.DatumOperace)
            .Select(h => new PosudekRoHistorieDetailDto
            {
                TypOperace = itemsById.TryGetValue(h.TypOperacePolozkaId, out var item) ? PosudekRoDtoFactory.ToCiselnikPolozkaWithTranslationsDto(item) : null,
                DatumOperace = h.DatumOperace,
                Lekar = new ZdravotnickyPracovnikDetailDto { KrzpId = h.KrzpIdLekare },
                Poskytovatel = new PoskytovatelZdravotnickychSluzebDetailDto { Ico = h.IcoPoskytovatele, Nazev = "PZS" }
            })
            .ToList();
    }
}

