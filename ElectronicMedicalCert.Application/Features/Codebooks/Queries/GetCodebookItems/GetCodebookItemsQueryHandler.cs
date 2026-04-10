using AutoMapper;
using ElectronicMedicalCert.Application.Common.Interfaces;
using ElectronicMedicalCert.Application.Contracts.V2;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Features.Codebooks.Queries.GetCodebookItems;

public sealed class GetCodebookItemsQueryHandler(IElectronicMedicalCertDbContext db, IMapper mapper)
    : IRequestHandler<GetCodebookItemsQuery, IReadOnlyList<CiselnikPolozkaDto>>
{
    public async Task<IReadOnlyList<CiselnikPolozkaDto>> Handle(GetCodebookItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await db.CodebookItems
            .AsNoTracking()
            .Include(x => x.Codebook)
            .Include(x => x.Preklady)
            .Where(x => x.Codebook.Kod == request.Kod)
            .OrderBy(x => x.Kod)
            .ToListAsync(cancellationToken);

        return items.Select(mapper.Map<CiselnikPolozkaDto>).ToList();
    }
}

