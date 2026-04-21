using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CreatePosudekRo;

public sealed class CreatePosudekRoCommandValidator : AbstractValidator<CreatePosudekRoCommand>
{
    public CreatePosudekRoCommandValidator()
        : this(null)
    {
    }

    public CreatePosudekRoCommandValidator(IElectronicMedicalCertDbContext? db)
    {
        RuleFor(x => x.Dto).NotNull();

        RuleFor(x => x.Dto.Rid)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Dto.KrzpId)
            .NotEmpty();

        RuleFor(x => x.Dto.DatumVystaveni)
            .NotEmpty();

        // use DB-aware validators (DI will inject db into this validator)
        RuleFor(x => x.Dto.TypAkce).SetValidator(new CiselnikPolozkaCreateValidator(db, "TYP-AKCE"));
        RuleFor(x => x.Dto.StavPosudku).SetValidator(new CiselnikPolozkaCreateValidator(db, "STAV-POSUDKU"));
        RuleFor(x => x.Dto.DruhProhlidky).SetValidator(new CiselnikPolozkaCreateValidator(db, "DRUH-PROHLIDKY"));
        RuleFor(x => x.Dto.DruhPosudku).SetValidator(new CiselnikPolozkaCreateValidator(db, "DRUH-POSUDKU"));

        RuleFor(x => x.Dto.Zpusobilosti)
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.Dto.Zpusobilosti).SetValidator(new ZpusobilostCreateValidator(db));
    }

    private sealed class CiselnikPolozkaCreateValidator : AbstractValidator<PosudekRoCiselnikPolozkaCreateDto>
    {
        public CiselnikPolozkaCreateValidator(IElectronicMedicalCertDbContext? db, string codebookKod)
        {
            RuleFor(x => x.Kod).NotEmpty();
            RuleFor(x => x.Verze).NotEmpty();

            if (db is not null)
            {
                RuleFor(x => x).MustAsync(async (dto, ct) =>
                {
                    if (dto is null || string.IsNullOrWhiteSpace(dto.Kod)) return false;
                    return await db.CodebookItems
                        .AsNoTracking()
                        .Include(i => i.Codebook)
                        .AnyAsync(i => i.Codebook.Kod == codebookKod && i.Kod == dto.Kod, ct);
                }).WithMessage(x => $"Value '{x?.Kod}' does not exist in codebook {codebookKod}.");
            }
        }
    }

    private sealed class ZpusobilostCreateValidator : AbstractValidator<PosudekRoZpusobilostCreateDto>
    {
        public ZpusobilostCreateValidator(IElectronicMedicalCertDbContext db)
        {
            RuleFor(x => x.SkupinaZadateleRidic).SetValidator(new CiselnikPolozkaCreateValidator(db, "SKUPINA-ZADATEL-RIDIC"));
            RuleFor(x => x.Vysledek).SetValidator(new CiselnikPolozkaCreateValidator(db, "VYSLEDEK"));

            RuleFor(x => x.SkupinyRidicskehoOpravneni)
                .NotNull()
                .NotEmpty();

            RuleForEach(x => x.SkupinyRidicskehoOpravneni)
                .ChildRules(g =>
                {
                    g.RuleFor(y => y.SkupinaRo).SetValidator(new CiselnikPolozkaCreateValidator(db, "SKUPINA-RO"));
                });

            RuleForEach(x => x.HarmonizovaneKody)
                .ChildRules(h =>
                {
                    h.RuleFor(y => y.HarmonizovanyKod).SetValidator(new CiselnikPolozkaCreateValidator(db, "HARMON-KOD"));
                    h.RuleFor(y => y.UpresneniText).MaximumLength(200);
                });

            RuleForEach(x => x.NarodniKody)
                .ChildRules(n =>
                {
                    n.RuleFor(y => y.NarodniKod).SetValidator(new CiselnikPolozkaCreateValidator(db, "NAROD-KOD"));
                    n.RuleFor(y => y.SkupinaRo).SetValidator(new CiselnikPolozkaCreateValidator(db, "SKUPINA-RO"));
                    n.RuleFor(y => y.UpresneniText).MaximumLength(200);
                });
        }
    }
}

