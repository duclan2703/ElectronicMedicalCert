namespace ElectronicMedicalCert.Application.Contracts.V2;

public sealed class PacientDto
{
    public string? Rid { get; set; }
    public string? Jmeno { get; set; }
    public string? Prijmeni { get; set; }
    public DateOnly DatumNarozeni { get; set; }
    public string? Adresa { get; set; }
    public string? Email { get; set; }
    public string? Doklad { get; set; }
    public string? Pohlavi { get; set; }
}

public sealed class ZdravotnickyPracovnikDetailDto
{
    public string? KrzpId { get; set; }
    public string? TitulPred { get; set; }
    public string? Jmeno { get; set; }
    public string? Prijmeni { get; set; }
    public string? TitulZa { get; set; }
    public string? Odbornost { get; set; }
}

public sealed class PoskytovatelZdravotnickychSluzebDetailDto
{
    public string? Ico { get; set; }
    public string? Nazev { get; set; }
    public string? Adresa { get; set; }
}

public sealed class PosudekRoCiselnikPolozkaCreateDto
{
    public string? Kod { get; set; }
    public string? Verze { get; set; }
}

public sealed class PosudekRoSkupinaCreateDto
{
    public PosudekRoCiselnikPolozkaCreateDto SkupinaRo { get; set; } = new();
}

public sealed class PosudekRoHarmonizovanyKodCreateDto
{
    public PosudekRoCiselnikPolozkaCreateDto HarmonizovanyKod { get; set; } = new();
    public List<PosudekRoCiselnikPolozkaCreateDto>? SkupinaRo { get; set; }
    public string? UpresneniText { get; set; }
}

public sealed class PosudekRoNarodniKodCreateDto
{
    public PosudekRoCiselnikPolozkaCreateDto NarodniKod { get; set; } = new();
    public PosudekRoCiselnikPolozkaCreateDto SkupinaRo { get; set; } = new();
    public string? UpresneniText { get; set; }
}

public sealed class PosudekRoZpusobilostCreateDto
{
    public PosudekRoCiselnikPolozkaCreateDto SkupinaZadateleRidic { get; set; } = new();
    public List<PosudekRoSkupinaCreateDto> SkupinyRidicskehoOpravneni { get; set; } = new();
    public PosudekRoCiselnikPolozkaCreateDto Vysledek { get; set; } = new();
    public List<PosudekRoHarmonizovanyKodCreateDto>? HarmonizovaneKody { get; set; }
    public List<PosudekRoNarodniKodCreateDto>? NarodniKody { get; set; }
}

public sealed class PosudekRoCreateDto
{
    public string Rid { get; set; } = default!;
    public string KrzpId { get; set; } = default!;
    public PosudekRoCiselnikPolozkaCreateDto TypAkce { get; set; } = new();
    public PosudekRoCiselnikPolozkaCreateDto StavPosudku { get; set; } = new();
    public PosudekRoCiselnikPolozkaCreateDto DruhProhlidky { get; set; } = new();
    public PosudekRoCiselnikPolozkaCreateDto DruhPosudku { get; set; } = new();
    public DateOnly DatumVystaveni { get; set; }
    public DateOnly? PlatnostDo { get; set; }
    public Guid? OpakovanyPosudekId { get; set; }
    public List<PosudekRoZpusobilostCreateDto> Zpusobilosti { get; set; } = new();
}

public sealed class PosudekRoHarmonizovanyKodDetailDto
{
    public CiselnikPolozkaWithTranslationsDto? HarmonizovanyKod { get; set; }
    public List<CiselnikPolozkaWithTranslationsDto>? SkupinaRo { get; set; }
    public CiselnikPolozkaWithTranslationsDto? UpresneniKod { get; set; }
    public string? UpresneniText { get; set; }
}

public sealed class PosudekRoNarodniKodDetailDto
{
    public CiselnikPolozkaWithTranslationsDto? NarodniKod { get; set; }
    public CiselnikPolozkaWithTranslationsDto? SkupinaRo { get; set; }
    public string? UpresneniText { get; set; }
}

public sealed class PosudekRoSkupinaDetailDto
{
    public CiselnikPolozkaWithTranslationsDto? SkupinaRo { get; set; }
}

public sealed class PosudekRoSkupinaZpusobilostDto
{
    public Guid PosudekId { get; set; }
    public CiselnikPolozkaWithTranslationsDto? SkupinaZadateleRidic { get; set; }
    public List<PosudekRoSkupinaDetailDto>? SkupinyRidicskehoOpravneni { get; set; }
    public CiselnikPolozkaWithTranslationsDto? Vysledek { get; set; }
    public List<PosudekRoHarmonizovanyKodDetailDto>? HarmonizovaneKody { get; set; }
    public List<PosudekRoNarodniKodDetailDto>? NarodniKody { get; set; }
    public string? VerzeZaznamu { get; set; }
}

public sealed class PosudekRoHlavickaDetailDto
{
    public PacientDto? Pacient { get; set; }
    public ZdravotnickyPracovnikDetailDto? ZdravotnickyPracovnik { get; set; }
    public PoskytovatelZdravotnickychSluzebDetailDto? PoskytovatelZdravotnickychSluzeb { get; set; }
    public CiselnikPolozkaWithTranslationsDto? OdbornostLekare { get; set; }
    public CiselnikPolozkaWithTranslationsDto? StavPosudku { get; set; }
    public CiselnikPolozkaWithTranslationsDto? DruhProhlidky { get; set; }
    public CiselnikPolozkaWithTranslationsDto? DruhPosudku { get; set; }
    public DateOnly DatumVystaveni { get; set; }
    public DateOnly? PlatnostDo { get; set; }
    public DateTime DatumVytvoreni { get; set; }
}

public sealed class PosudekRoCreateResultDto
{
    public PosudekRoHlavickaDetailDto? Hlavicka { get; set; }
    public List<PosudekRoSkupinaZpusobilostDto>? Zpusobilosti { get; set; }
}

public sealed class PosudekRoDetailDto
{
    public Guid Id { get; set; }
    public PacientDto? Pacient { get; set; }
    public ZdravotnickyPracovnikDetailDto? ZdravotnickyPracovnik { get; set; }
    public PoskytovatelZdravotnickychSluzebDetailDto? PoskytovatelZdravotnickychSluzeb { get; set; }
    public CiselnikPolozkaWithTranslationsDto? OdbornostLekare { get; set; }
    public CiselnikPolozkaWithTranslationsDto? StavPosudku { get; set; }
    public CiselnikPolozkaWithTranslationsDto? DruhProhlidky { get; set; }
    public CiselnikPolozkaWithTranslationsDto? DruhPosudku { get; set; }
    public CiselnikPolozkaWithTranslationsDto? Vysledek { get; set; }
    public CiselnikPolozkaWithTranslationsDto? SkupinaZadatelRidic { get; set; }
    public DateOnly DatumVystaveni { get; set; }
    public DateOnly? PlatnostDo { get; set; }
    public DateTime DatumVytvoreni { get; set; }
    public List<PosudekRoSkupinaDetailDto>? SkupinyRidicskehoOpravneni { get; set; }
    public List<PosudekRoHarmonizovanyKodDetailDto>? HarmonizovaneKody { get; set; }
    public List<PosudekRoNarodniKodDetailDto>? NarodniKody { get; set; }
    public string? VerzeZaznamu { get; set; }
}

public sealed class PosudekRoHistorieDetailDto
{
    public CiselnikPolozkaWithTranslationsDto? TypOperace { get; set; }
    public DateTime DatumOperace { get; set; }
    public ZdravotnickyPracovnikDetailDto? Lekar { get; set; }
    public PoskytovatelZdravotnickychSluzebDetailDto? Poskytovatel { get; set; }
}

public sealed class PosudkyRoSearchRequest
{
    public string? Rid { get; set; }
    public DateTime? DatumOd { get; set; }
    public DateTime? DatumDo { get; set; }
    public bool? JenPlatne { get; set; }
    public Guid? StavPosudku { get; set; }
    public string? Fulltext { get; set; }
    public string? Ico { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public string? Sort { get; set; }
    public string? Order { get; set; }
}

public sealed class PosudekRoDetailDtoPageResponse
{
    public int PageNumber { get; set; }
    public int PageCount { get; set; }
    public int? NextPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public List<PosudekRoDetailDto>? Page { get; set; }
}

public sealed class PosudekRoOpravneniRequestDto
{
    public string? Ico { get; set; }
    public string? KrzpId { get; set; }
}

public sealed class PosudekRoOpravneniResponseDto
{
    public bool Opravneni { get; set; }
}

