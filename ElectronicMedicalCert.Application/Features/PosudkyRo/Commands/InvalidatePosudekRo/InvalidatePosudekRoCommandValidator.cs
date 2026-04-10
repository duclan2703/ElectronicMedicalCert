using FluentValidation;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.InvalidatePosudekRo;

public sealed class InvalidatePosudekRoCommandValidator : AbstractValidator<InvalidatePosudekRoCommand>
{
    public InvalidatePosudekRoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.IfMatch).NotEmpty();
    }
}

