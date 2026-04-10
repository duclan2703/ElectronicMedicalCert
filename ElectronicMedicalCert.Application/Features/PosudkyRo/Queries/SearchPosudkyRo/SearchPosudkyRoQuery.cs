using ElectronicMedicalCert.Application.Contracts.V2;
using MediatR;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.SearchPosudkyRo;

public sealed record SearchPosudkyRoQuery(PosudkyRoSearchRequest Dto) : IRequest<PosudekRoDetailDtoPageResponse>;

