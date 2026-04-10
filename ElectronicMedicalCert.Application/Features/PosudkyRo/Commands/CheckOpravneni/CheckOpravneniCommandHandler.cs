using ElectronicMedicalCert.Application.Common.Interfaces;
using ElectronicMedicalCert.Application.Contracts.V2;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CheckOpravneni;

public sealed class CheckOpravneniCommandHandler(IElectronicMedicalCertDbContext db)
    : IRequestHandler<CheckOpravneniCommand, PosudekRoOpravneniResponseDto>
{
    public async Task<PosudekRoOpravneniResponseDto> Handle(CheckOpravneniCommand request, CancellationToken cancellationToken)
    {
        var ok = await db.AuthorizedWorkers
            .AsNoTracking()
            .AnyAsync(x => x.Ico == request.Dto.Ico && x.KrzpId == request.Dto.KrzpId, cancellationToken);

        return new PosudekRoOpravneniResponseDto { Opravneni = ok };
    }
}

