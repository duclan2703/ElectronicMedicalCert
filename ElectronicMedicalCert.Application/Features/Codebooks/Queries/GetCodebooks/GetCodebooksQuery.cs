using ElectronicMedicalCert.Application.Contracts.V2;
using MediatR;

namespace ElectronicMedicalCert.Application.Features.Codebooks.Queries.GetCodebooks;

public sealed record GetCodebooksQuery : IRequest<IReadOnlyList<CiselnikDto>>;

