namespace ElectronicMedicalCert.Domain.Entities;

public sealed class PosudekRoZpusobilost
{
    public Guid Id { get; set; }

    public Guid PosudekRoId { get; set; }
    public PosudekRo PosudekRo { get; set; } = default!;

    public Guid SkupinaZadateleRidicPolozkaId { get; set; }
    public Guid VysledekPolozkaId { get; set; }

    public byte[] RowVersion { get; set; } = default!;

    public List<PosudekRoSkupina> SkupinyRidicskehoOpravneni { get; set; } = new();
    public List<PosudekRoHarmonizovanyKod> HarmonizovaneKody { get; set; } = new();
    public List<PosudekRoNarodniKod> NarodniKody { get; set; } = new();
}

public sealed class PosudekRoSkupina
{
    public Guid Id { get; set; }
    public Guid PosudekRoZpusobilostId { get; set; }
    public PosudekRoZpusobilost PosudekRoZpusobilost { get; set; } = default!;

    public Guid SkupinaRoPolozkaId { get; set; }
}

public sealed class PosudekRoHarmonizovanyKod
{
    public Guid Id { get; set; }
    public Guid PosudekRoZpusobilostId { get; set; }
    public PosudekRoZpusobilost PosudekRoZpusobilost { get; set; } = default!;

    public Guid HarmonizovanyKodPolozkaId { get; set; }
    public string? UpresneniText { get; set; }

    public List<PosudekRoHarmonizovanyKodSkupinaRo> SkupinaRo { get; set; } = new();
}

public sealed class PosudekRoHarmonizovanyKodSkupinaRo
{
    public Guid Id { get; set; }
    public Guid PosudekRoHarmonizovanyKodId { get; set; }
    public PosudekRoHarmonizovanyKod PosudekRoHarmonizovanyKod { get; set; } = default!;

    public Guid SkupinaRoPolozkaId { get; set; }
}

public sealed class PosudekRoNarodniKod
{
    public Guid Id { get; set; }
    public Guid PosudekRoZpusobilostId { get; set; }
    public PosudekRoZpusobilost PosudekRoZpusobilost { get; set; } = default!;

    public Guid NarodniKodPolozkaId { get; set; }
    public Guid SkupinaRoPolozkaId { get; set; }
    public string? UpresneniText { get; set; }
}

