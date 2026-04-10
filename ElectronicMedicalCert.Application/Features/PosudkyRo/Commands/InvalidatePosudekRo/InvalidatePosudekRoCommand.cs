using ElectronicMedicalCert.Application.Contracts.V2;
using MediatR;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.InvalidatePosudekRo;

public sealed record InvalidatePosudekRoCommand(Guid Id, string IfMatch) : IRequest<PosudekRoDetailDto>;

