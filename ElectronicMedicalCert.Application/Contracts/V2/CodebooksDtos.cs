namespace ElectronicMedicalCert.Application.Contracts.V2;

public sealed class TranslationDto
{
    public string? Nazev { get; set; }
    public string? Popis { get; set; }
}

public sealed class CiselnikDto
{
    public Guid Id { get; set; }
    public string? Kod { get; set; }
    public string? Verze { get; set; }
    public DateTime PlatnostOd { get; set; }
    public DateTime? PlatnostDo { get; set; }
    public bool? Termx { get; set; }
    public string? TermxId { get; set; }
    public string? TermxUrl { get; set; }
    public Dictionary<string, TranslationDto>? Preklady { get; set; }
}

public sealed class CiselnikPolozkaDto
{
    public Guid Id { get; set; }
    public string? Kod { get; set; }
    public string? Verze { get; set; }
    public Guid? RodicId { get; set; }
    public Dictionary<string, TranslationDto>? Preklady { get; set; }
}

public sealed class CiselnikPolozkaWithTranslationsDto
{
    public string? CiselnikKod { get; set; }
    public string? CiselnikVerze { get; set; }
    public string? PolozkaKod { get; set; }
    public Dictionary<string, TranslationDto>? Preklady { get; set; }
}

