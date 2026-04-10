using FluentValidation;

namespace ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.SearchPosudkyRo;

public sealed class SearchPosudkyRoQueryValidator : AbstractValidator<SearchPosudkyRoQuery>
{
    public SearchPosudkyRoQueryValidator()
    {
        RuleFor(x => x.Dto).NotNull();
        RuleFor(x => x.Dto.Page).GreaterThan(0);
        RuleFor(x => x.Dto.Size).InclusiveBetween(1, 200);
        RuleFor(x => x.Dto.Rid).MaximumLength(20);
        RuleFor(x => x.Dto.Sort).MaximumLength(50);
        RuleFor(x => x.Dto.Order).MaximumLength(10);
        RuleFor(x => x.Dto.Ico).MaximumLength(20);
    }
}

