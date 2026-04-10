using AutoMapper;
using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Domain.Entities;

namespace ElectronicMedicalCert.Application.Common.Mapping;

public sealed class CodebookProfile : Profile
{
    public CodebookProfile()
    {
        CreateMap<Codebook, CiselnikDto>()
            .ForMember(d => d.Preklady, o => o.MapFrom(s => TranslationMapping.ToDictionary(s.Preklady)));

        CreateMap<CodebookItem, CiselnikPolozkaDto>()
            .ForMember(d => d.Preklady, o => o.MapFrom(s => TranslationMapping.ToDictionary(s.Preklady)));
    }
}

