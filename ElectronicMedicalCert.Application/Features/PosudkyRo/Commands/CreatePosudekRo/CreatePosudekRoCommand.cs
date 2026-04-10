using ElectronicMedicalCert.Application.Contracts.V2;
using MediatR;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CreatePosudekRo;

public sealed record CreatePosudekRoCommand(PosudekRoCreateDto Dto) : IRequest<PosudekRoCreateResultDto>;

