using ElectronicMedicalCert.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ElectronicMedicalCertDbContext db, CancellationToken ct = default)
    {
        if (await db.Codebooks.AsNoTracking().AnyAsync(ct))
        {
            return;
        }

        db.AuthorizedWorkers.Add(new AuthorizedWorker
        {
            Id = SeedIds.AuthorizedWorker1,
            Ico = "12345678",
            KrzpId = "KRZP-001",
        });

        var codebooks = new List<Codebook>
        {
            NewCb(SeedIds.CbStavPosudku, "STAV-POSUDKU", "Stav posudku", "Certificate state"),
            NewCb(SeedIds.CbTypAkce, "TYP-AKCE", "Typ akce", "Action type"),
            NewCb(SeedIds.CbDruhProhlidky, "DRUH-PROHLIDKY", "Druh prohlídky", "Exam type"),
            NewCb(SeedIds.CbDruhPosudku, "DRUH-POSUDKU", "Druh posudku", "Certificate type"),
            NewCb(SeedIds.CbVysledek, "VYSLEDEK", "Výsledek", "Result"),
            NewCb(SeedIds.CbSkupinaZadateleRidic, "SKUPINA-ZADATEL-RIDIC", "Skupina žadatele", "Applicant group"),
            NewCb(SeedIds.CbSkupinaRo, "SKUPINA-RO", "Skupiny ŘO", "Driving licence groups"),
            NewCb(SeedIds.CbTypOperace, "TYP-OPERACE", "Typ operace", "Operation type"),
            NewCb(SeedIds.CbOdbornost, "ODBORNOST-LEKARE", "Odbornost", "Specialization"),
            NewCb(SeedIds.CbHarmonKod, "HARMON-KOD", "Harmonizované kódy", "Harmonized codes"),
            NewCb(SeedIds.CbNarodKod, "NAROD-KOD", "Národní kódy", "National codes"),
        };

        db.Codebooks.AddRange(codebooks);

        db.CodebookItems.AddRange(new[]
        {
            NewItem(SeedIds.ItStavPlatny, SeedIds.CbStavPosudku, "PLATNY", "Platný", "Valid"),
            NewItem(SeedIds.ItStavZneplatneny, SeedIds.CbStavPosudku, "ZNEPLATNENY", "Zneplatněný", "Invalidated"),
            NewItem(SeedIds.ItAkceVytvoreni, SeedIds.CbTypAkce, "VYTVORENI", "Vytvoření", "Create"),
            NewItem(SeedIds.ItAkceZneplatneni, SeedIds.CbTypAkce, "ZNEPLATNENI", "Zneplatnění", "Invalidate"),
            NewItem(SeedIds.ItDruhProhlidkyVstupni, SeedIds.CbDruhProhlidky, "VSTUPNI", "Vstupní", "Initial"),
            NewItem(SeedIds.ItDruhPosudkuRo, SeedIds.CbDruhPosudku, "RO", "Řidičské oprávnění", "Driving licence"),
            NewItem(SeedIds.ItVysledekZpusobily, SeedIds.CbVysledek, "ZPUSOBILY", "Způsobilý", "Fit"),
            NewItem(SeedIds.ItVysledekNezpusobily, SeedIds.CbVysledek, "NEZPUSOBILY", "Nezpůsobilý", "Unfit"),
            NewItem(SeedIds.ItSkupinaZadatelRidic, SeedIds.CbSkupinaZadateleRidic, "RIDIC", "Řidič", "Driver"),
            NewItem(SeedIds.ItSkupinaRoB, SeedIds.CbSkupinaRo, "B", "B", "B"),
            NewItem(SeedIds.ItOperaceCreate, SeedIds.CbTypOperace, "CREATE", "Vytvoření", "Create"),
            NewItem(SeedIds.ItOperaceInvalidate, SeedIds.CbTypOperace, "INVALIDATE", "Zneplatnění", "Invalidate"),
            NewItem(SeedIds.ItOdbornostVseobecny, SeedIds.CbOdbornost, "VSEOBECNY", "Všeobecný lékař", "General practitioner"),
            NewItem(SeedIds.ItHarmon01, SeedIds.CbHarmonKod, "01", "01", "01"),
            NewItem(SeedIds.ItNarod01, SeedIds.CbNarodKod, "N01", "N01", "N01"),
        });

        await db.SaveChangesAsync(ct);
    }

    private static Codebook NewCb(Guid id, string kod, string csName, string enName) =>
        new()
        {
            Id = id,
            Kod = kod,
            Verze = "1",
            PlatnostOd = SeedIds.ValidFrom,
            Preklady =
            {
                new Translation { Id = Guid.NewGuid(), Language = "cs", Nazev = csName },
                new Translation { Id = Guid.NewGuid(), Language = "en", Nazev = enName },
            }
        };

    private static CodebookItem NewItem(Guid id, Guid cbId, string kod, string csName, string enName) =>
        new()
        {
            Id = id,
            CodebookId = cbId,
            Kod = kod,
            Verze = "1",
            Preklady =
            {
                new Translation { Id = Guid.NewGuid(), Language = "cs", Nazev = csName },
                new Translation { Id = Guid.NewGuid(), Language = "en", Nazev = enName },
            }
        };
}

