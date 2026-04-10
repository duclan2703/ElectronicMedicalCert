namespace ElectronicMedicalCert.Domain.Entities;

public sealed class PosudekRoHistorie
{
    public Guid Id { get; set; }
    public Guid PosudekRoId { get; set; }
    public PosudekRo PosudekRo { get; set; } = default!;

    public Guid TypOperacePolozkaId { get; set; }
    public DateTime DatumOperace { get; set; }

    public string KrzpIdLekare { get; set; } = default!;
    public string IcoPoskytovatele { get; set; } = default!;
}

