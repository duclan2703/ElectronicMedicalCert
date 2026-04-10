using ElectronicMedicalCert.Application.Common.Interfaces;
using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.SearchPosudkyRo;

public sealed class SearchPosudkyRoQueryHandler(IElectronicMedicalCertDbContext db)
    : IRequestHandler<SearchPosudkyRoQuery, PosudekRoDetailDtoPageResponse>
{
    public async Task<PosudekRoDetailDtoPageResponse> Handle(SearchPosudkyRoQuery request, CancellationToken cancellationToken)
    {
        var q = db.PosudkyRo.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Dto.Rid))
        {
            q = q.Where(p => p.Rid == request.Dto.Rid);
        }

        if (!string.IsNullOrWhiteSpace(request.Dto.Ico))
        {
            q = q.Where(p => p.Ico == request.Dto.Ico);
        }

        if (request.Dto.DatumOd is not null)
        {
            q = q.Where(p => p.DatumVytvoreni >= request.Dto.DatumOd.Value);
        }

        if (request.Dto.DatumDo is not null)
        {
            q = q.Where(p => p.DatumVytvoreni <= request.Dto.DatumDo.Value);
        }

        if (request.Dto.StavPosudku is not null)
        {
            q = q.Where(p => p.StavPosudkuPolozkaId == request.Dto.StavPosudku.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Dto.Fulltext))
        {
            var ft = request.Dto.Fulltext.Trim();
            q = q.Where(p => p.Rid.Contains(ft) || p.KrzpId.Contains(ft) || p.Ico.Contains(ft));
        }

        if (request.Dto.JenPlatne == true)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            q = q.Where(p => p.PlatnostDo == null || p.PlatnostDo >= today);
        }

        q = ApplySorting(q, request.Dto.Sort, request.Dto.Order);

        var total = await q.CountAsync(cancellationToken);
        var pageSize = request.Dto.Size;
        var pageNumber = request.Dto.Page;
        var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)pageSize);
        var skip = (pageNumber - 1) * pageSize;

        var posudky = await q
            .Skip(skip)
            .Take(pageSize)
            .Include(p => p.Zpusobilosti)
                .ThenInclude(z => z.SkupinyRidicskehoOpravneni)
            .Include(p => p.Zpusobilosti)
                .ThenInclude(z => z.HarmonizovaneKody)
                    .ThenInclude(h => h.SkupinaRo)
            .Include(p => p.Zpusobilosti)
                .ThenInclude(z => z.NarodniKody)
            .ToListAsync(cancellationToken);

        var usedIds = new HashSet<Guid>();
        foreach (var p in posudky)
        {
            usedIds.Add(p.StavPosudkuPolozkaId);
            usedIds.Add(p.DruhProhlidkyPolozkaId);
            usedIds.Add(p.DruhPosudkuPolozkaId);
            foreach (var z in p.Zpusobilosti)
            {
                usedIds.Add(z.SkupinaZadateleRidicPolozkaId);
                usedIds.Add(z.VysledekPolozkaId);
                foreach (var s in z.SkupinyRidicskehoOpravneni) usedIds.Add(s.SkupinaRoPolozkaId);
                foreach (var h in z.HarmonizovaneKody)
                {
                    usedIds.Add(h.HarmonizovanyKodPolozkaId);
                    foreach (var sr in h.SkupinaRo) usedIds.Add(sr.SkupinaRoPolozkaId);
                }
                foreach (var n in z.NarodniKody)
                {
                    usedIds.Add(n.NarodniKodPolozkaId);
                    usedIds.Add(n.SkupinaRoPolozkaId);
                }
            }
        }

        var lookup = new CodebookLookup(db);
        var itemsById = await lookup.LoadItemsByIds(usedIds, cancellationToken);

        var page = posudky.Select(p => PosudekRoDtoFactory.ToDetail(p, itemsById)).ToList();

        return new PosudekRoDetailDtoPageResponse
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = total,
            PageCount = pageCount,
            NextPage = pageNumber < pageCount ? pageNumber + 1 : null,
            Page = page,
        };
    }

    private static IQueryable<Domain.Entities.PosudekRo> ApplySorting(IQueryable<Domain.Entities.PosudekRo> q, string? sort, string? order)
    {
        var desc = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);

        return (sort?.ToLowerInvariant()) switch
        {
            "datumvystaveni" => desc ? q.OrderByDescending(x => x.DatumVystaveni) : q.OrderBy(x => x.DatumVystaveni),
            "datumvytvoreni" => desc ? q.OrderByDescending(x => x.DatumVytvoreni) : q.OrderBy(x => x.DatumVytvoreni),
            "rid" => desc ? q.OrderByDescending(x => x.Rid) : q.OrderBy(x => x.Rid),
            _ => q.OrderByDescending(x => x.DatumVytvoreni),
        };
    }
}

