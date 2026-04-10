namespace ElectronicMedicalCert.Domain.Entities;

public sealed class CodebookItem
{
    public Guid Id { get; set; }
    public Guid CodebookId { get; set; }
    public Codebook Codebook { get; set; } = default!;

    public string? Kod { get; set; }
    public string? Verze { get; set; }
    public Guid? RodicId { get; set; }

    public List<Translation> Preklady { get; set; } = new();
}

