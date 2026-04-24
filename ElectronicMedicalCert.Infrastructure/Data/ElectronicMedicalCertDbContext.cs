using ElectronicMedicalCert.Application.Common.Interfaces;
using ElectronicMedicalCert.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalCert.Infrastructure.Data;

public sealed class ElectronicMedicalCertDbContext(DbContextOptions<ElectronicMedicalCertDbContext> options) : DbContext(options), IElectronicMedicalCertDbContext
{
    public DbSet<Codebook> Codebooks => Set<Codebook>();
    public DbSet<CodebookItem> CodebookItems => Set<CodebookItem>();
    public DbSet<AuthorizedWorker> AuthorizedWorkers => Set<AuthorizedWorker>();

    public DbSet<PosudekRo> PosudkyRo => Set<PosudekRo>();
    public DbSet<PosudekRoZpusobilost> PosudkyRoZpusobilosti => Set<PosudekRoZpusobilost>();
    public DbSet<PosudekRoHistorie> PosudkyRoHistorie => Set<PosudekRoHistorie>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        StampRowVersions();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void StampRowVersions()
    {
        foreach (var e in ChangeTracker.Entries<PosudekRo>().Where(e => e.State is EntityState.Added))
        {
            e.Entity.RowVersion = Guid.NewGuid().ToByteArray();
        }

        foreach (var e in ChangeTracker.Entries<PosudekRoZpusobilost>().Where(e => e.State is EntityState.Added))
        {
            e.Entity.RowVersion = Guid.NewGuid().ToByteArray();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Codebook>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Kod).HasMaxLength(100);
            b.Property(x => x.Verze).HasMaxLength(50);
            b.Property(x => x.TermxId).HasMaxLength(100);
            b.Property(x => x.TermxUrl).HasMaxLength(500);

            b.HasMany(x => x.Polozky)
                .WithOne(x => x.Codebook)
                .HasForeignKey(x => x.CodebookId)
                .OnDelete(DeleteBehavior.Cascade);

            b.OwnsMany(x => x.Preklady, tb =>
            {
                tb.ToTable("CodebookTranslations");
                tb.HasKey(x => x.Id);
                tb.Property(x => x.Language).HasMaxLength(10).IsRequired();
                tb.Property(x => x.Nazev).HasMaxLength(200);
                tb.Property(x => x.Popis).HasMaxLength(1000);
                tb.WithOwner().HasForeignKey("CodebookId");
            });
        });

        modelBuilder.Entity<CodebookItem>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Kod).HasMaxLength(100);
            b.Property(x => x.Verze).HasMaxLength(50);

            b.OwnsMany(x => x.Preklady, tb =>
            {
                tb.ToTable("CodebookItemTranslations");
                tb.HasKey(x => x.Id);
                tb.Property(x => x.Language).HasMaxLength(10).IsRequired();
                tb.Property(x => x.Nazev).HasMaxLength(200);
                tb.Property(x => x.Popis).HasMaxLength(1000);
                tb.WithOwner().HasForeignKey("CodebookItemId");
            });
        });

        modelBuilder.Entity<AuthorizedWorker>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Ico).HasMaxLength(20).IsRequired();
            b.Property(x => x.KrzpId).HasMaxLength(50).IsRequired();
            b.HasIndex(x => new { x.Ico, x.KrzpId }).IsUnique();
        });

        modelBuilder.Entity<PosudekRo>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Rid).HasMaxLength(20).IsRequired();
            b.Property(x => x.KrzpId).HasMaxLength(50).IsRequired();
            b.Property(x => x.Ico).HasMaxLength(20).IsRequired();
            b.Property(x => x.RowVersion).IsRequired().IsRowVersion();

            b.HasMany(x => x.Zpusobilosti)
                .WithOne(x => x.PosudekRo)
                .HasForeignKey(x => x.PosudekRoId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(x => x.Historie)
                .WithOne(x => x.PosudekRo)
                .HasForeignKey(x => x.PosudekRoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PosudekRoZpusobilost>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.RowVersion).IsRequired().IsRowVersion();

            b.HasMany(x => x.SkupinyRidicskehoOpravneni)
                .WithOne(x => x.PosudekRoZpusobilost)
                .HasForeignKey(x => x.PosudekRoZpusobilostId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(x => x.HarmonizovaneKody)
                .WithOne(x => x.PosudekRoZpusobilost)
                .HasForeignKey(x => x.PosudekRoZpusobilostId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(x => x.NarodniKody)
                .WithOne(x => x.PosudekRoZpusobilost)
                .HasForeignKey(x => x.PosudekRoZpusobilostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PosudekRoSkupina>(b => b.HasKey(x => x.Id));
        modelBuilder.Entity<PosudekRoHarmonizovanyKod>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.UpresneniText).HasMaxLength(200);
            b.HasMany(x => x.SkupinaRo)
                .WithOne(x => x.PosudekRoHarmonizovanyKod)
                .HasForeignKey(x => x.PosudekRoHarmonizovanyKodId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<PosudekRoHarmonizovanyKodSkupinaRo>(b => b.HasKey(x => x.Id));
        modelBuilder.Entity<PosudekRoNarodniKod>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.UpresneniText).HasMaxLength(200);
        });

        modelBuilder.Entity<PosudekRoHistorie>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.KrzpIdLekare).HasMaxLength(50).IsRequired();
            b.Property(x => x.IcoPoskytovatele).HasMaxLength(20).IsRequired();
        });

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorizedWorker>().HasData(new AuthorizedWorker
        {
            Id = SeedIds.AuthorizedWorker1,
            Ico = "12345678",
            KrzpId = "KRZP-001",
        });

        modelBuilder.Entity<Codebook>().HasData(
            new Codebook
            {
                Id = SeedIds.CbStavPosudku,
                Kod = "STAV-POSUDKU",
                Verze = "1",
                PlatnostOd = SeedIds.ValidFrom,
            },
            new Codebook
            {
                Id = SeedIds.CbTypAkce,
                Kod = "TYP-AKCE",
                Verze = "1",
                PlatnostOd = SeedIds.ValidFrom,
            },
            new Codebook
            {
                Id = SeedIds.CbDruhProhlidky,
                Kod = "DRUH-PROHLIDKY",
                Verze = "1",
                PlatnostOd = SeedIds.ValidFrom,
            },
            new Codebook
            {
                Id = SeedIds.CbDruhPosudku,
                Kod = "DRUH-POSUDKU",
                Verze = "1",
                PlatnostOd = SeedIds.ValidFrom,
            },
            new Codebook
            {
                Id = SeedIds.CbVysledek,
                Kod = "VYSLEDEK",
                Verze = "1",
                PlatnostOd = SeedIds.ValidFrom,
            },
            new Codebook
            {
                Id = SeedIds.CbSkupinaZadateleRidic,
                Kod = "SKUPINA-ZADATEL-RIDIC",
                Verze = "1",
                PlatnostOd = SeedIds.ValidFrom,
            },
            new Codebook
            {
                Id = SeedIds.CbSkupinaRo,
                Kod = "SKUPINA-RO",
                Verze = "1",
                PlatnostOd = SeedIds.ValidFrom,
            },
            new Codebook
            {
                Id = SeedIds.CbTypOperace,
                Kod = "TYP-OPERACE",
                Verze = "1",
                PlatnostOd = SeedIds.ValidFrom,
            },
            new Codebook
            {
                Id = SeedIds.CbOdbornost,
                Kod = "ODBORNOST-LEKARE",
                Verze = "1",
                PlatnostOd = SeedIds.ValidFrom,
            },
            new Codebook
            {
                Id = SeedIds.CbHarmonKod,
                Kod = "HARMON-KOD",
                Verze = "1",
                PlatnostOd = SeedIds.ValidFrom,
            },
            new Codebook
            {
                Id = SeedIds.CbNarodKod,
                Kod = "NAROD-KOD",
                Verze = "1",
                PlatnostOd = SeedIds.ValidFrom,
            }
        );

        modelBuilder.Entity<Codebook>().OwnsMany(x => x.Preklady).HasData(
            SeedTranslation(SeedIds.CbStavPosudku, SeedIds.CbStavPosudkuTrCs, "cs", "Stav posudku", "Stavy posudků."),
            SeedTranslation(SeedIds.CbStavPosudku, SeedIds.CbStavPosudkuTrEn, "en", "Certificate state", "Certificate states."),
            SeedTranslation(SeedIds.CbTypAkce, SeedIds.CbTypAkceTrCs, "cs", "Typ akce", null),
            SeedTranslation(SeedIds.CbTypAkce, SeedIds.CbTypAkceTrEn, "en", "Action type", null),
            SeedTranslation(SeedIds.CbDruhProhlidky, SeedIds.CbDruhProhlidkyTrCs, "cs", "Druh prohlídky", null),
            SeedTranslation(SeedIds.CbDruhProhlidky, SeedIds.CbDruhProhlidkyTrEn, "en", "Exam type", null),
            SeedTranslation(SeedIds.CbDruhPosudku, SeedIds.CbDruhPosudkuTrCs, "cs", "Druh posudku", null),
            SeedTranslation(SeedIds.CbDruhPosudku, SeedIds.CbDruhPosudkuTrEn, "en", "Certificate type", null),
            SeedTranslation(SeedIds.CbVysledek, SeedIds.CbVysledekTrCs, "cs", "Výsledek", null),
            SeedTranslation(SeedIds.CbVysledek, SeedIds.CbVysledekTrEn, "en", "Result", null),
            SeedTranslation(SeedIds.CbSkupinaZadateleRidic, SeedIds.CbSkupinaZadateleRidicTrCs, "cs", "Skupina žadatele", null),
            SeedTranslation(SeedIds.CbSkupinaZadateleRidic, SeedIds.CbSkupinaZadateleRidicTrEn, "en", "Applicant group", null),
            SeedTranslation(SeedIds.CbSkupinaRo, SeedIds.CbSkupinaRoTrCs, "cs", "Skupiny ŘO", null),
            SeedTranslation(SeedIds.CbSkupinaRo, SeedIds.CbSkupinaRoTrEn, "en", "Driving licence groups", null),
            SeedTranslation(SeedIds.CbTypOperace, SeedIds.CbTypOperaceTrCs, "cs", "Typ operace", null),
            SeedTranslation(SeedIds.CbTypOperace, SeedIds.CbTypOperaceTrEn, "en", "Operation type", null),
            SeedTranslation(SeedIds.CbOdbornost, SeedIds.CbOdbornostTrCs, "cs", "Odbornost", null),
            SeedTranslation(SeedIds.CbOdbornost, SeedIds.CbOdbornostTrEn, "en", "Specialization", null),
            SeedTranslation(SeedIds.CbHarmonKod, SeedIds.CbHarmonKodTrCs, "cs", "Harmonizované kódy", null),
            SeedTranslation(SeedIds.CbHarmonKod, SeedIds.CbHarmonKodTrEn, "en", "Harmonized codes", null),
            SeedTranslation(SeedIds.CbNarodKod, SeedIds.CbNarodKodTrCs, "cs", "Národní kódy", null),
            SeedTranslation(SeedIds.CbNarodKod, SeedIds.CbNarodKodTrEn, "en", "National codes", null)
        );

        modelBuilder.Entity<CodebookItem>().HasData(
            new CodebookItem { Id = SeedIds.ItStavPlatny, CodebookId = SeedIds.CbStavPosudku, Kod = "stav_posudku_platny", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItStavZneplatneny, CodebookId = SeedIds.CbStavPosudku, Kod = "stav_posudku_zneplatneny", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItAkceVytvoreni, CodebookId = SeedIds.CbTypAkce, Kod = "akce_ro_vytvoreni", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItAkceZneplatneni, CodebookId = SeedIds.CbTypAkce, Kod = "akce_ro_zneplatneni", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItDruhProhlidkyVstupni, CodebookId = SeedIds.CbDruhProhlidky, Kod = "druh_prohlidky_ro_vstupni", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItDruhProhlidkyPeriodicka, CodebookId = SeedIds.CbDruhProhlidky, Kod = "druh_prohlidky_ro_periodicka", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItDruhProhlidkyMimoradna, CodebookId = SeedIds.CbDruhProhlidky, Kod = "druh_prohlidky_ro_mimoradna", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItDruhPosudkuRo, CodebookId = SeedIds.CbDruhPosudku, Kod = "druh_posudku_ro", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItVysledekZpusobily, CodebookId = SeedIds.CbVysledek, Kod = "vysledek_posudku_ro_zpusobily", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItVysledekNezpusobily, CodebookId = SeedIds.CbVysledek, Kod = "vysledek_posudku_ro_nezpusobily", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItVysledekZpusobilySPodminkou, CodebookId = SeedIds.CbVysledek, Kod = "vysledek_posudku_ro_zpusobily_s_podminkou", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItSkupinaZadatelRidic, CodebookId = SeedIds.CbSkupinaZadateleRidic, Kod = "skupina_ro_1", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItSkupinaZadatelRidic2, CodebookId = SeedIds.CbSkupinaZadateleRidic, Kod = "skupina_ro_2", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItSkupinaRoB, CodebookId = SeedIds.CbSkupinaRo, Kod = "B", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItOperaceCreate, CodebookId = SeedIds.CbTypOperace, Kod = "CREATE", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItOperaceInvalidate, CodebookId = SeedIds.CbTypOperace, Kod = "INVALIDATE", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItOdbornostVseobecny, CodebookId = SeedIds.CbOdbornost, Kod = "VSEOBECNY", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItHarmon01, CodebookId = SeedIds.CbHarmonKod, Kod = "01", Verze = "1" },
            new CodebookItem { Id = SeedIds.ItNarod01, CodebookId = SeedIds.CbNarodKod, Kod = "N01", Verze = "1" }
        );

        modelBuilder.Entity<CodebookItem>().OwnsMany(x => x.Preklady).HasData(
            SeedItemTranslation(SeedIds.ItStavPlatny, SeedIds.ItStavPlatnyTrCs, "cs", "Platný", null),
            SeedItemTranslation(SeedIds.ItStavPlatny, SeedIds.ItStavPlatnyTrEn, "en", "Valid", null),
            SeedItemTranslation(SeedIds.ItStavZneplatneny, SeedIds.ItStavZneplatnenyTrCs, "cs", "Zneplatněný", null),
            SeedItemTranslation(SeedIds.ItStavZneplatneny, SeedIds.ItStavZneplatnenyTrEn, "en", "Invalidated", null),
            SeedItemTranslation(SeedIds.ItAkceVytvoreni, SeedIds.ItAkceVytvoreniTrCs, "cs", "Vytvoření", null),
            SeedItemTranslation(SeedIds.ItAkceVytvoreni, SeedIds.ItAkceVytvoreniTrEn, "en", "Creation", null),
            SeedItemTranslation(SeedIds.ItAkceZneplatneni, SeedIds.ItAkceZneplatneniTrCs, "cs", "Zneplatnění", null),
            SeedItemTranslation(SeedIds.ItAkceZneplatneni, SeedIds.ItAkceZneplatneniTrEn, "en", "Invalidation", null),
            SeedItemTranslation(SeedIds.ItDruhProhlidkyVstupni, SeedIds.ItDruhProhlidkyVstupniTrCs, "cs", "Vstupní prohlídka", null),
            SeedItemTranslation(SeedIds.ItDruhProhlidkyVstupni, SeedIds.ItDruhProhlidkyVstupniTrEn, "en", "Initial examination", null),
            SeedItemTranslation(SeedIds.ItDruhPosudkuRo, SeedIds.ItDruhPosudkuRoTrCs, "cs", "Řidičské oprávnění", null),
            SeedItemTranslation(SeedIds.ItDruhPosudkuRo, SeedIds.ItDruhPosudkuRoTrEn, "en", "Driving licence", null),
            SeedItemTranslation(SeedIds.ItVysledekZpusobily, SeedIds.ItVysledekZpusobilyTrCs, "cs", "Způsobilý", null),
            SeedItemTranslation(SeedIds.ItVysledekZpusobily, SeedIds.ItVysledekZpusobilyTrEn, "en", "Fit", null),
            SeedItemTranslation(SeedIds.ItVysledekNezpusobily, SeedIds.ItVysledekNezpusobilyTrCs, "cs", "Nezpůsobilý", null),
            SeedItemTranslation(SeedIds.ItVysledekNezpusobily, SeedIds.ItVysledekNezpusobilyTrEn, "en", "Unfit", null),
            SeedItemTranslation(SeedIds.ItSkupinaZadatelRidic, SeedIds.ItSkupinaZadatelRidicTrCs, "cs", "Skupina 1", null),
            SeedItemTranslation(SeedIds.ItSkupinaZadatelRidic, SeedIds.ItSkupinaZadatelRidicTrEn, "en", "Group 1", null),
            SeedItemTranslation(SeedIds.ItSkupinaRoB, SeedIds.ItSkupinaRoBTrCs, "cs", "B", null),
            SeedItemTranslation(SeedIds.ItSkupinaRoB, SeedIds.ItSkupinaRoBTrEn, "en", "B", null),
            SeedItemTranslation(SeedIds.ItOperaceCreate, SeedIds.ItOperaceCreateTrCs, "cs", "Vytvoření", null),
            SeedItemTranslation(SeedIds.ItOperaceCreate, SeedIds.ItOperaceCreateTrEn, "en", "Create", null),
            SeedItemTranslation(SeedIds.ItOperaceInvalidate, SeedIds.ItOperaceInvalidateTrCs, "cs", "Zneplatnění", null),
            SeedItemTranslation(SeedIds.ItOperaceInvalidate, SeedIds.ItOperaceInvalidateTrEn, "en", "Invalidate", null),
            SeedItemTranslation(SeedIds.ItOdbornostVseobecny, SeedIds.ItOdbornostVseobecnyTrCs, "cs", "Všeobecný lékař", null),
            SeedItemTranslation(SeedIds.ItOdbornostVseobecny, SeedIds.ItOdbornostVseobecnyTrEn, "en", "General practitioner", null),
            SeedItemTranslation(SeedIds.ItHarmon01, SeedIds.ItHarmon01TrCs, "cs", "01", null),
            SeedItemTranslation(SeedIds.ItHarmon01, SeedIds.ItHarmon01TrEn, "en", "01", null),
            SeedItemTranslation(SeedIds.ItNarod01, SeedIds.ItNarod01TrCs, "cs", "N01", null),
            SeedItemTranslation(SeedIds.ItNarod01, SeedIds.ItNarod01TrEn, "en", "N01", null)
        );
    }

    private static object SeedTranslation(Guid codebookId, Guid id, string lang, string? nazev, string? popis) =>
        new
        {
            Id = id,
            CodebookId = codebookId,
            Language = lang,
            Nazev = nazev,
            Popis = popis,
        };

    private static object SeedItemTranslation(Guid itemId, Guid id, string lang, string? nazev, string? popis) =>
        new
        {
            Id = id,
            CodebookItemId = itemId,
            Language = lang,
            Nazev = nazev,
            Popis = popis,
        };
}

