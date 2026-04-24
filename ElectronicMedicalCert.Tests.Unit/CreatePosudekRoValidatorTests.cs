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
            TypAkce = new PosudekRoCiselnikPolozkaCreateDto { Kod = "akce_ro_vytvoreni", Verze = "1" },
            StavPosudku = new PosudekRoCiselnikPolozkaCreateDto { Kod = "stav_posudku_platny", Verze = "1" },
            DruhProhlidky = new PosudekRoCiselnikPolozkaCreateDto { Kod = "druh_prohlidky_ro_vstupni", Verze = "1" },
            DruhPosudku = new PosudekRoCiselnikPolozkaCreateDto { Kod = "druh_posudku_ro", Verze = "1" },
            DatumVystaveni = new DateOnly(2026, 1, 1),
            Zpusobilosti =
            {
                new PosudekRoZpusobilostCreateDto
                {
                    SkupinaZadateleRidic = new PosudekRoCiselnikPolozkaCreateDto { Kod = "skupina_ro_1", Verze = "1" },
                    Vysledek = new PosudekRoCiselnikPolozkaCreateDto { Kod = "vysledek_posudku_ro_zpusobily", Verze = "1" },
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

