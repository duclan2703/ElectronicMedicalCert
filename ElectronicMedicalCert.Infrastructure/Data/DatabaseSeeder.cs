using ElectronicMedicalCert.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElectronicMedicalCert.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ElectronicMedicalCertDbContext db, ILogger logger, bool enableTermxFetch = true, CancellationToken ct = default)
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
            NewCb(SeedIds.CbStavPosudku, "STAV-POSUDKU", "Stav posudku", "Certificate state", termxName: "elp-ro-stav-posudku"),
            NewCb(SeedIds.CbTypAkce, "TYP-AKCE", "Typ akce", "Action type", termxName: "elp-ro-akce"),
            NewCb(SeedIds.CbDruhProhlidky, "DRUH-PROHLIDKY", "Druh prohlídky", "Exam type", termxName: "elp-ro-druh-prohlidky"),
            NewCb(SeedIds.CbDruhPosudku, "DRUH-POSUDKU", "Druh posudku", "Certificate type", termxName: "elp-ro-druh-posudku"),
            NewCb(SeedIds.CbVysledek, "VYSLEDEK", "Výsledek", "Result", termxName: "elp-ro-vysledek-posudku"),
            NewCb(SeedIds.CbSkupinaZadateleRidic, "SKUPINA-ZADATEL-RIDIC", "Skupina žadatele", "Applicant group", termxName: "elp-ro-zadatel-skupina"),
            NewCb(SeedIds.CbSkupinaRo, "SKUPINA-RO", "Skupiny ŘO", "Driving licence groups", termxName: "elp-ro-seznam-skupin"),
            NewCb(SeedIds.CbTypOperace, "TYP-OPERACE", "Typ operace", "Operation type"),
            NewCb(SeedIds.CbOdbornost, "ODBORNOST-LEKARE", "Odbornost", "Specialization"),
            NewCb(SeedIds.CbHarmonKod, "HARMON-KOD", "Harmonizované kódy", "Harmonized codes", termxName: "elp-ro-harmonizovane-kody"),
            NewCb(SeedIds.CbNarodKod, "NAROD-KOD", "Národní kódy", "National codes", termxName: "elp-ro-narodni-kody"),
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

        // attempt to fetch TermX concepts for codebooks that have TermxUrl set
        if (enableTermxFetch)
        {
            logger.LogInformation("TermX fetching enabled - starting TermX concept synchronization");
            await FetchAndSeedTermxConceptsAsync(db, logger, ct);
            logger.LogInformation("TermX concept synchronization finished");
        }
        else
        {
            logger.LogInformation("TermX fetching disabled by configuration; skipping TermX synchronization");
        }
    }

    private static Codebook NewCb(Guid id, string kod, string csName, string enName, string? termxName = null) =>
        new()
        {
            Id = id,
            Kod = kod,
            Verze = "1",
            PlatnostOd = SeedIds.ValidFrom,
            Termx = termxName is not null,
            TermxId = termxName,
            TermxUrl = termxName is not null ? $"https://terminologie.ezdravi.gov.cz/resources/value-sets/{termxName}/versions/1.0.0/concepts" : null,
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

    public static async Task FetchAndSeedTermxConceptsAsync(ElectronicMedicalCertDbContext db, ILogger logger, CancellationToken ct = default)
    {
        var http = new HttpClient();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var cbWithTermx = await db.Codebooks.AsNoTracking().Where(cb => cb.Termx == true && !string.IsNullOrWhiteSpace(cb.TermxUrl)).ToListAsync(ct);
        foreach (var cb in cbWithTermx)
        {
            try
            {
                logger.LogInformation("Fetching TermX concepts for codebook {Codebook} from {Url}", cb.Kod, cb.TermxUrl);
                var resp = await http.GetAsync(cb.TermxUrl, ct);
                if (!resp.IsSuccessStatusCode)
                {
                    logger.LogWarning("TermX request for {Codebook} returned status {Status}", cb.Kod, resp.StatusCode);
                    continue;
                }

                using var s = await resp.Content.ReadAsStreamAsync(ct);
                var doc = await JsonDocument.ParseAsync(s, cancellationToken: ct);

                // TermX concepts endpoint structure can vary; try to find array named "concepts" or top-level array
                JsonElement conceptsElem;
                if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("concepts", out conceptsElem) && conceptsElem.ValueKind == JsonValueKind.Array)
                {
                    // ok
                }
                else if (doc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    conceptsElem = doc.RootElement;
                }
                else
                {
                    continue;
                }

                foreach (var item in conceptsElem.EnumerateArray())
                {
                    // extract code
                    var code = item.TryGetProperty("code", out var c) ? c.GetString()
                        : item.TryGetProperty("id", out var idp) ? idp.GetString()
                        : null;

                    // extract labels: prefer multilingual structures, fall back to display/prefLabel/label
                    string? csLabel = null;
                    string? enLabel = null;

                    if (item.TryGetProperty("displayMultilingual", out var dm) && dm.ValueKind == JsonValueKind.Object)
                    {
                        if (dm.TryGetProperty("cs", out var dcs) && dcs.ValueKind == JsonValueKind.String) csLabel = dcs.GetString();
                        if (dm.TryGetProperty("en", out var den) && den.ValueKind == JsonValueKind.String) enLabel = den.GetString();
                    }

                    if (string.IsNullOrWhiteSpace(csLabel) && item.TryGetProperty("display", out var d) && d.ValueKind == JsonValueKind.String)
                    {
                        csLabel = d.GetString();
                    }

                    if (string.IsNullOrWhiteSpace(enLabel) && item.TryGetProperty("displayEn", out var den2) && den2.ValueKind == JsonValueKind.String)
                    {
                        enLabel = den2.GetString();
                    }

                    // try other common properties
                    if (string.IsNullOrWhiteSpace(csLabel) && item.TryGetProperty("prefLabel", out var pl) && pl.ValueKind == JsonValueKind.Object)
                    {
                        if (pl.TryGetProperty("cs", out var plcs) && plcs.ValueKind == JsonValueKind.String) csLabel = plcs.GetString();
                        if (pl.TryGetProperty("en", out var plen) && plen.ValueKind == JsonValueKind.String) enLabel = plen.GetString();
                    }

                    if (string.IsNullOrWhiteSpace(csLabel) && item.TryGetProperty("label", out var lab) && lab.ValueKind == JsonValueKind.String)
                    {
                        csLabel = lab.GetString();
                    }

                    if (string.IsNullOrWhiteSpace(code)) continue;

                    var exists = await db.CodebookItems.AsNoTracking().AnyAsync(i => i.CodebookId == cb.Id && i.Kod == code, ct);
                    if (exists) continue;

                    var newItem = new CodebookItem { Id = Guid.NewGuid(), CodebookId = cb.Id, Kod = code, Verze = "1" };
                    // add translations
                    if (!string.IsNullOrWhiteSpace(csLabel)) newItem.Preklady.Add(new Translation { Id = Guid.NewGuid(), Language = "cs", Nazev = csLabel });
                    if (!string.IsNullOrWhiteSpace(enLabel)) newItem.Preklady.Add(new Translation { Id = Guid.NewGuid(), Language = "en", Nazev = enLabel });

                    db.CodebookItems.Add(newItem);
                }

                await db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                // log exception and continue so startup is not blocked by transient TermX issues
                try
                {
                    logger.LogError(ex, "Failed to fetch or seed TermX concepts for codebook {Codebook} from {Url}", cb.Kod, cb.TermxUrl);
                }
                catch
                {
                    // swallow any logging errors to avoid secondary failures
                }
            }
        }
    }
}

