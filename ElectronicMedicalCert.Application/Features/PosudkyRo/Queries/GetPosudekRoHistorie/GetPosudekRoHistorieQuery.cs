using ElectronicMedicalCert.Application.Contracts.V2;
using MediatR;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoHistorie;

public sealed record GetPosudekRoHistorieQuery(Guid Id) : IRequest<IReadOnlyList<PosudekRoHistorieDetailDto>>;

