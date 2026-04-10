namespace ElectronicMedicalCert.Domain.Entities;

public sealed class AuthorizedWorker
{
    public Guid Id { get; set; }
    public string Ico { get; set; } = default!;
    public string KrzpId { get; set; } = default!;
}

