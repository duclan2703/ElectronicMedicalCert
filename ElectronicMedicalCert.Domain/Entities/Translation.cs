namespace ElectronicMedicalCert.Domain.Entities;

public sealed class Translation
{
    public Guid Id { get; set; }
    public string Language { get; set; } = default!;
    public string? Nazev { get; set; }
    public string? Popis { get; set; }
}

