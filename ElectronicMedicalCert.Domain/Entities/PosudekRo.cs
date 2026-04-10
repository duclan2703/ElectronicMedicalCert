namespace ElectronicMedicalCert.Domain.Entities;

public sealed class PosudekRo
{
    public Guid Id { get; set; }

    public string Rid { get; set; } = default!;
    public string KrzpId { get; set; } = default!;
    public string Ico { get; set; } = default!;

    public DateOnly DatumVystaveni { get; set; }
    public DateOnly? PlatnostDo { get; set; }
    public DateTime DatumVytvoreni { get; set; }
    public Guid? OpakovanyPosudekId { get; set; }

    public Guid TypAkcePolozkaId { get; set; }
    public Guid StavPosudkuPolozkaId { get; set; }
    public Guid DruhProhlidkyPolozkaId { get; set; }
    public Guid DruhPosudkuPolozkaId { get; set; }

    public byte[] RowVersion { get; set; } = default!;

    public List<PosudekRoZpusobilost> Zpusobilosti { get; set; } = new();
    public List<PosudekRoHistorie> Historie { get; set; } = new();
}

