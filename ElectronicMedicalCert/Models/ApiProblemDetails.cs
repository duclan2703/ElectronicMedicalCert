namespace ElectronicMedicalCert.Models;

public sealed class ApiProblemDetails
{
    public string? Type { get; init; }
    public string? Title { get; init; }
    public int Status { get; init; }
    public string? Detail { get; init; }
    public string? Instance { get; init; }
    public string? CorrelationId { get; init; }
    public IReadOnlyList<ApiProblemFieldError>? Errors { get; init; }
}

public sealed class ApiProblemFieldError
{
    public string? Field { get; init; }
    public string? Message { get; init; }
}

