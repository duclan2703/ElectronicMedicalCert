using ElectronicMedicalCert.Application.Common.Exceptions;
using ElectronicMedicalCert.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoPdf;

public sealed class GetPosudekRoPdfQueryHandler(IElectronicMedicalCertDbContext db)
    : IRequestHandler<GetPosudekRoPdfQuery, GetPosudekRoPdfResult>
{
    public async Task<GetPosudekRoPdfResult> Handle(GetPosudekRoPdfQuery request, CancellationToken cancellationToken)
    {
        var exists = await db.PosudkyRo.AsNoTracking().AnyAsync(p => p.Id == request.Id, cancellationToken);
        if (!exists)
        {
            throw new NotFoundException("Požadovaný posudek nebyl nalezen.");
        }

        var content = MinimalPdf($"Posudek ŘO {request.Id}");
        return new GetPosudekRoPdfResult(content, $"posudek-{request.Id}.pdf");
    }

    // Minimal PDF generator (placeholder) good enough for API contract tests.
    private static byte[] MinimalPdf(string text)
    {
        // A tiny PDF containing only a single page and a text.
        // This is intentionally minimal and not feature-complete.
        var safeText = text.Replace("(", "\\(").Replace(")", "\\)");
        var pdf = $@"%PDF-1.4
1 0 obj<< /Type /Catalog /Pages 2 0 R >>endobj
2 0 obj<< /Type /Pages /Kids [3 0 R] /Count 1 >>endobj
3 0 obj<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Contents 4 0 R /Resources<< /Font<< /F1 5 0 R >> >> >>endobj
4 0 obj<< /Length 74 >>stream
BT /F1 18 Tf 72 720 Td ({safeText}) Tj ET
endstream endobj
5 0 obj<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>endobj
xref
0 6
0000000000 65535 f 
0000000010 00000 n 
0000000060 00000 n 
0000000116 00000 n 
0000000242 00000 n 
0000000366 00000 n 
trailer<< /Size 6 /Root 1 0 R >>
startxref
450
%%EOF";
        return System.Text.Encoding.ASCII.GetBytes(pdf);
    }
}

