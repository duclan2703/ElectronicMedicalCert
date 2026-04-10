using FluentValidation;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CheckOpravneni;

public sealed class CheckOpravneniCommandValidator : AbstractValidator<CheckOpravneniCommand>
{
    public CheckOpravneniCommandValidator()
    {
        RuleFor(x => x.Dto).NotNull();
        RuleFor(x => x.Dto.Ico).NotEmpty();
        RuleFor(x => x.Dto.KrzpId).NotEmpty();
    }
}

