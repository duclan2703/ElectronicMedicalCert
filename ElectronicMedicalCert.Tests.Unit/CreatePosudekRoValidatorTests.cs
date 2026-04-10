using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CreatePosudekRo;
using FluentAssertions;
using Xunit;

namespace ElectronicMedicalCert.Tests.Unit;

public sealed class CreatePosudekRoValidatorTests
{
    [Fact]
    public void Rejects_empty_rid()
    {
        var validator = new CreatePosudekRoCommandValidator();
        var cmd = new CreatePosudekRoCommand(new PosudekRoCreateDto
        {
            Rid = "",
            KrzpId = "KRZP-001",
            TypAkce = new PosudekRoCiselnikPolozkaCreateDto { Kod = "VYTVORENI", Verze = "1" },
            StavPosudku = new PosudekRoCiselnikPolozkaCreateDto { Kod = "PLATNY", Verze = "1" },
            DruhProhlidky = new PosudekRoCiselnikPolozkaCreateDto { Kod = "VSTUPNI", Verze = "1" },
            DruhPosudku = new PosudekRoCiselnikPolozkaCreateDto { Kod = "RO", Verze = "1" },
            DatumVystaveni = new DateOnly(2026, 1, 1),
            Zpusobilosti =
            {
                new PosudekRoZpusobilostCreateDto
                {
                    SkupinaZadateleRidic = new PosudekRoCiselnikPolozkaCreateDto { Kod = "RIDIC", Verze = "1" },
                    Vysledek = new PosudekRoCiselnikPolozkaCreateDto { Kod = "ZPUSOBILY", Verze = "1" },
                    SkupinyRidicskehoOpravneni =
                    {
                        new PosudekRoSkupinaCreateDto { SkupinaRo = new PosudekRoCiselnikPolozkaCreateDto { Kod = "B", Verze = "1" } }
                    }
                }
            }
        });

        var res = validator.Validate(cmd);
        res.IsValid.Should().BeFalse();
        res.Errors.Should().Contain(e => e.PropertyName.Contains("Rid"));
    }
}

