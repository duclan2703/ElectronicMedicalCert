using MediatR;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoPdf;

public sealed record GetPosudekRoPdfQuery(Guid Id) : IRequest<GetPosudekRoPdfResult>;

public sealed record GetPosudekRoPdfResult(byte[] Content, string FileName);

