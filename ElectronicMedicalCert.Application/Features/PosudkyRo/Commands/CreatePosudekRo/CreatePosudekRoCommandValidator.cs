using ElectronicMedicalCert.Application.Contracts.V2;
using FluentValidation;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CreatePosudekRo;

public sealed class CreatePosudekRoCommandValidator : AbstractValidator<CreatePosudekRoCommand>
{
    public CreatePosudekRoCommandValidator()
    {
        RuleFor(x => x.Dto).NotNull();

        RuleFor(x => x.Dto.Rid)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Dto.KrzpId)
            .NotEmpty();

        RuleFor(x => x.Dto.DatumVystaveni)
            .NotEmpty();

        RuleFor(x => x.Dto.TypAkce).SetValidator(new CiselnikPolozkaCreateValidator());
        RuleFor(x => x.Dto.StavPosudku).SetValidator(new CiselnikPolozkaCreateValidator());
        RuleFor(x => x.Dto.DruhProhlidky).SetValidator(new CiselnikPolozkaCreateValidator());
        RuleFor(x => x.Dto.DruhPosudku).SetValidator(new CiselnikPolozkaCreateValidator());

        RuleFor(x => x.Dto.Zpusobilosti)
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.Dto.Zpusobilosti).SetValidator(new ZpusobilostCreateValidator());
    }

    private sealed class CiselnikPolozkaCreateValidator : AbstractValidator<PosudekRoCiselnikPolozkaCreateDto>
    {
        public CiselnikPolozkaCreateValidator()
        {
            RuleFor(x => x.Kod).NotEmpty();
            RuleFor(x => x.Verze).NotEmpty();
        }
    }

    private sealed class ZpusobilostCreateValidator : AbstractValidator<PosudekRoZpusobilostCreateDto>
    {
        public ZpusobilostCreateValidator()
        {
            RuleFor(x => x.SkupinaZadateleRidic).SetValidator(new CiselnikPolozkaCreateValidator());
            RuleFor(x => x.Vysledek).SetValidator(new CiselnikPolozkaCreateValidator());

            RuleFor(x => x.SkupinyRidicskehoOpravneni)
                .NotNull()
                .NotEmpty();

            RuleForEach(x => x.SkupinyRidicskehoOpravneni)
                .ChildRules(g =>
                {
                    g.RuleFor(y => y.SkupinaRo).SetValidator(new CiselnikPolozkaCreateValidator());
                });

            RuleForEach(x => x.HarmonizovaneKody)
                .ChildRules(h =>
                {
                    h.RuleFor(y => y.HarmonizovanyKod).SetValidator(new CiselnikPolozkaCreateValidator());
                    h.RuleFor(y => y.UpresneniText).MaximumLength(200);
                });

            RuleForEach(x => x.NarodniKody)
                .ChildRules(n =>
                {
                    n.RuleFor(y => y.NarodniKod).SetValidator(new CiselnikPolozkaCreateValidator());
                    n.RuleFor(y => y.SkupinaRo).SetValidator(new CiselnikPolozkaCreateValidator());
                    n.RuleFor(y => y.UpresneniText).MaximumLength(200);
                });
        }
    }
}

