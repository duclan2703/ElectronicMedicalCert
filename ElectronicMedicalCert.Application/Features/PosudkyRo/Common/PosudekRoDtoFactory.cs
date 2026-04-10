using ElectronicMedicalCert.Application.Common.Mapping;
using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Domain.Entities;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Common;

internal static class PosudekRoDtoFactory
{
    public static CiselnikPolozkaWithTranslationsDto? ToCiselnikPolozkaWithTranslationsDto(CodebookItem? item)
    {
        if (item is null) return null;

        return new CiselnikPolozkaWithTranslationsDto
        {
            CiselnikKod = item.Codebook?.Kod,
            CiselnikVerze = item.Codebook?.Verze,
            PolozkaKod = item.Kod,
            Preklady = TranslationMapping.ToDictionary(item.Preklady),
        };
    }

    public static PosudekRoDetailDto ToDetail(PosudekRo posudek, Dictionary<Guid, CodebookItem> itemsById)
    {
        var firstZp = posudek.Zpusobilosti.FirstOrDefault();

        CodebookItem? Item(Guid id) => itemsById.TryGetValue(id, out var i) ? i : null;

        var pacient = new PacientDto
        {
            Rid = posudek.Rid,
            DatumNarozeni = new DateOnly(1980, 1, 1),
        };

        var lekar = new ZdravotnickyPracovnikDetailDto
        {
            KrzpId = posudek.KrzpId,
        };

        var poskytovatel = new PoskytovatelZdravotnickychSluzebDetailDto
        {
            Ico = posudek.Ico,
            Nazev = "PZS",
        };

        return new PosudekRoDetailDto
        {
            Id = posudek.Id,
            Pacient = pacient,
            ZdravotnickyPracovnik = lekar,
            PoskytovatelZdravotnickychSluzeb = poskytovatel,
            OdbornostLekare = null,
            StavPosudku = ToCiselnikPolozkaWithTranslationsDto(Item(posudek.StavPosudkuPolozkaId)),
            DruhProhlidky = ToCiselnikPolozkaWithTranslationsDto(Item(posudek.DruhProhlidkyPolozkaId)),
            DruhPosudku = ToCiselnikPolozkaWithTranslationsDto(Item(posudek.DruhPosudkuPolozkaId)),
            DatumVystaveni = posudek.DatumVystaveni,
            PlatnostDo = posudek.PlatnostDo,
            DatumVytvoreni = posudek.DatumVytvoreni,
            VerzeZaznamu = Convert.ToBase64String(posudek.RowVersion),

            SkupinaZadatelRidic = firstZp is null ? null : ToCiselnikPolozkaWithTranslationsDto(Item(firstZp.SkupinaZadateleRidicPolozkaId)),
            Vysledek = firstZp is null ? null : ToCiselnikPolozkaWithTranslationsDto(Item(firstZp.VysledekPolozkaId)),
            SkupinyRidicskehoOpravneni = firstZp?.SkupinyRidicskehoOpravneni
                .Select(s => new PosudekRoSkupinaDetailDto { SkupinaRo = ToCiselnikPolozkaWithTranslationsDto(Item(s.SkupinaRoPolozkaId)) })
                .ToList(),
            HarmonizovaneKody = firstZp?.HarmonizovaneKody
                .Select(h => new PosudekRoHarmonizovanyKodDetailDto
                {
                    HarmonizovanyKod = ToCiselnikPolozkaWithTranslationsDto(Item(h.HarmonizovanyKodPolozkaId)),
                    SkupinaRo = h.SkupinaRo.Select(s => ToCiselnikPolozkaWithTranslationsDto(Item(s.SkupinaRoPolozkaId))!).ToList(),
                    UpresneniText = h.UpresneniText,
                })
                .ToList(),
            NarodniKody = firstZp?.NarodniKody
                .Select(n => new PosudekRoNarodniKodDetailDto
                {
                    NarodniKod = ToCiselnikPolozkaWithTranslationsDto(Item(n.NarodniKodPolozkaId)),
                    SkupinaRo = ToCiselnikPolozkaWithTranslationsDto(Item(n.SkupinaRoPolozkaId)),
                    UpresneniText = n.UpresneniText,
                })
                .ToList(),
        };
    }

    public static PosudekRoCreateResultDto ToCreateResult(PosudekRo posudek, Dictionary<Guid, CodebookItem> itemsById)
    {
        var detail = ToDetail(posudek, itemsById);

        var zpusobilosti = posudek.Zpusobilosti.Select(zp => new PosudekRoSkupinaZpusobilostDto
        {
            PosudekId = posudek.Id,
            SkupinaZadateleRidic = itemsById.TryGetValue(zp.SkupinaZadateleRidicPolozkaId, out var sg) ? ToCiselnikPolozkaWithTranslationsDto(sg) : null,
            Vysledek = itemsById.TryGetValue(zp.VysledekPolozkaId, out var vy) ? ToCiselnikPolozkaWithTranslationsDto(vy) : null,
            SkupinyRidicskehoOpravneni = zp.SkupinyRidicskehoOpravneni.Select(s => new PosudekRoSkupinaDetailDto
            {
                SkupinaRo = itemsById.TryGetValue(s.SkupinaRoPolozkaId, out var it) ? ToCiselnikPolozkaWithTranslationsDto(it) : null
            }).ToList(),
            HarmonizovaneKody = zp.HarmonizovaneKody.Select(h => new PosudekRoHarmonizovanyKodDetailDto
            {
                HarmonizovanyKod = itemsById.TryGetValue(h.HarmonizovanyKodPolozkaId, out var it) ? ToCiselnikPolozkaWithTranslationsDto(it) : null,
                SkupinaRo = h.SkupinaRo.Select(sr => itemsById.TryGetValue(sr.SkupinaRoPolozkaId, out var sri) ? ToCiselnikPolozkaWithTranslationsDto(sri) : null).Where(x => x is not null).ToList()!,
                UpresneniText = h.UpresneniText
            }).ToList(),
            NarodniKody = zp.NarodniKody.Select(n => new PosudekRoNarodniKodDetailDto
            {
                NarodniKod = itemsById.TryGetValue(n.NarodniKodPolozkaId, out var it) ? ToCiselnikPolozkaWithTranslationsDto(it) : null,
                SkupinaRo = itemsById.TryGetValue(n.SkupinaRoPolozkaId, out var it2) ? ToCiselnikPolozkaWithTranslationsDto(it2) : null,
                UpresneniText = n.UpresneniText
            }).ToList(),
            VerzeZaznamu = Convert.ToBase64String(zp.RowVersion),
        }).ToList();

        return new PosudekRoCreateResultDto
        {
            Hlavicka = new PosudekRoHlavickaDetailDto
            {
                Pacient = detail.Pacient,
                ZdravotnickyPracovnik = detail.ZdravotnickyPracovnik,
                PoskytovatelZdravotnickychSluzeb = detail.PoskytovatelZdravotnickychSluzeb,
                OdbornostLekare = detail.OdbornostLekare,
                StavPosudku = detail.StavPosudku,
                DruhProhlidky = detail.DruhProhlidky,
                DruhPosudku = detail.DruhPosudku,
                DatumVystaveni = detail.DatumVystaveni,
                PlatnostDo = detail.PlatnostDo,
                DatumVytvoreni = detail.DatumVytvoreni,
            },
            Zpusobilosti = zpusobilosti,
        };
    }
}

