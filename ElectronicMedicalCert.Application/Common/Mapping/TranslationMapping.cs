using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Domain.Entities;

namespace ElectronicMedicalCert.Application.Common.Mapping;

internal static class TranslationMapping
{
    public static Dictionary<string, TranslationDto>? ToDictionary(List<Translation> translations)
    {
        if (translations.Count == 0) return null;

        return translations
            .GroupBy(t => t.Language)
            .ToDictionary(
                g => g.Key,
                g =>
                {
                    var t = g.First();
                    return new TranslationDto { Nazev = t.Nazev, Popis = t.Popis };
                });
    }
}

