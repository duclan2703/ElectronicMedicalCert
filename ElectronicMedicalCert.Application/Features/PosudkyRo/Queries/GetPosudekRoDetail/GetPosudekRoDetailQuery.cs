using ElectronicMedicalCert.Application.Contracts.V2;
using MediatR;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoDetail;

public sealed record GetPosudekRoDetailQuery(Guid Id) : IRequest<PosudekRoDetailDto>;

