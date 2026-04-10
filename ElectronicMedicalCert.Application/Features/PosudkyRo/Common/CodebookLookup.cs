using ElectronicMedicalCert.Application.Common.Interfaces;
using ElectronicMedicalCert.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Common;

internal sealed class CodebookLookup(IElectronicMedicalCertDbContext db)
{
    public async Task<CodebookItem> ResolveItem(string field, string codebookKod, string? itemKod, string? itemVerze, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(itemKod) || string.IsNullOrWhiteSpace(itemVerze))
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure(field, "Missing codebook item.")
            });
        }

        var item = await db.CodebookItems
            .Include(x => x.Codebook)
            .Include(x => x.Preklady)
            .Where(x => x.Codebook.Kod == codebookKod && x.Kod == itemKod && x.Verze == itemVerze)
            .FirstOrDefaultAsync(ct);

        if (item is null)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure(field, "Neplatná hodnota číselníku.")
            });
        }

        return item;
    }

    public async Task<Dictionary<Guid, CodebookItem>> LoadItemsByIds(IEnumerable<Guid> ids, CancellationToken ct)
    {
        var idList = ids.Distinct().ToList();
        if (idList.Count == 0) return new();

        var items = await db.CodebookItems
            .AsNoTracking()
            .Include(x => x.Codebook)
            .Include(x => x.Preklady)
            .Where(x => idList.Contains(x.Id))
            .ToListAsync(ct);

        return items.ToDictionary(x => x.Id);
    }
}

