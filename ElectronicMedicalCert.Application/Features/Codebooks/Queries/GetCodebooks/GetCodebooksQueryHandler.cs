using AutoMapper;
using ElectronicMedicalCert.Application.Common.Interfaces;
using ElectronicMedicalCert.Application.Contracts.V2;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Features.Codebooks.Queries.GetCodebooks;

public sealed class GetCodebooksQueryHandler(IElectronicMedicalCertDbContext db, IMapper mapper)
    : IRequestHandler<GetCodebooksQuery, IReadOnlyList<CiselnikDto>>
{
    public async Task<IReadOnlyList<CiselnikDto>> Handle(GetCodebooksQuery request, CancellationToken cancellationToken)
    {
        var codebooks = await db.Codebooks
            .AsNoTracking()
            .Include(x => x.Preklady)
            .OrderBy(x => x.Kod)
            .ToListAsync(cancellationToken);

        return codebooks.Select(mapper.Map<CiselnikDto>).ToList();
    }
}

