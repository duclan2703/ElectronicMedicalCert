namespace ElectronicMedicalCert.Domain.Entities;

public sealed class Codebook
{
    public Guid Id { get; set; }
    public string? Kod { get; set; }
    public string? Verze { get; set; }
    public DateTime PlatnostOd { get; set; }
    public DateTime? PlatnostDo { get; set; }
    public bool? Termx { get; set; }
    public string? TermxId { get; set; }
    public string? TermxUrl { get; set; }

    public List<Translation> Preklady { get; set; } = new();
    public List<CodebookItem> Polozky { get; set; } = new();
}

