using ElectronicMedicalCert.Application.Contracts.V2;
using MediatR;

namespace ElectronicMedicalCert.Application.Features.Codebooks.Queries.GetCodebookItems;

public sealed record GetCodebookItemsQuery(string Kod) : IRequest<IReadOnlyList<CiselnikPolozkaDto>>;

