using ElectronicMedicalCert.Application.Contracts.V2;
using MediatR;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CheckOpravneni;

public sealed record CheckOpravneniCommand(PosudekRoOpravneniRequestDto Dto) : IRequest<PosudekRoOpravneniResponseDto>;

