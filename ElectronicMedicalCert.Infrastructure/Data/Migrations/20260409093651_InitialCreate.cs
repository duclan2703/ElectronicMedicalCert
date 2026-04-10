using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ElectronicMedicalCert.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorizedWorkers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ico = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KrzpId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizedWorkers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Codebooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Verze = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PlatnostOd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlatnostDo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Termx = table.Column<bool>(type: "bit", nullable: true),
                    TermxId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TermxUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codebooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PosudkyRo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rid = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KrzpId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ico = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DatumVystaveni = table.Column<DateOnly>(type: "date", nullable: false),
                    PlatnostDo = table.Column<DateOnly>(type: "date", nullable: true),
                    DatumVytvoreni = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OpakovanyPosudekId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TypAkcePolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StavPosudkuPolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DruhProhlidkyPolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DruhPosudkuPolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosudkyRo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodebookItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodebookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Verze = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RodicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodebookItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodebookItems_Codebooks_CodebookId",
                        column: x => x.CodebookId,
                        principalTable: "Codebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodebookTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nazev = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Popis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CodebookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodebookTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodebookTranslations_Codebooks_CodebookId",
                        column: x => x.CodebookId,
                        principalTable: "Codebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PosudkyRoHistorie",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PosudekRoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypOperacePolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatumOperace = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KrzpIdLekare = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IcoPoskytovatele = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosudkyRoHistorie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosudkyRoHistorie_PosudkyRo_PosudekRoId",
                        column: x => x.PosudekRoId,
                        principalTable: "PosudkyRo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PosudkyRoZpusobilosti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PosudekRoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkupinaZadateleRidicPolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VysledekPolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosudkyRoZpusobilosti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosudkyRoZpusobilosti_PosudkyRo_PosudekRoId",
                        column: x => x.PosudekRoId,
                        principalTable: "PosudkyRo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodebookItemTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nazev = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Popis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CodebookItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodebookItemTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodebookItemTranslations_CodebookItems_CodebookItemId",
                        column: x => x.CodebookItemId,
                        principalTable: "CodebookItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PosudekRoHarmonizovanyKod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PosudekRoZpusobilostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HarmonizovanyKodPolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpresneniText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosudekRoHarmonizovanyKod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosudekRoHarmonizovanyKod_PosudkyRoZpusobilosti_PosudekRoZpusobilostId",
                        column: x => x.PosudekRoZpusobilostId,
                        principalTable: "PosudkyRoZpusobilosti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PosudekRoNarodniKod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PosudekRoZpusobilostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NarodniKodPolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkupinaRoPolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpresneniText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosudekRoNarodniKod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosudekRoNarodniKod_PosudkyRoZpusobilosti_PosudekRoZpusobilostId",
                        column: x => x.PosudekRoZpusobilostId,
                        principalTable: "PosudkyRoZpusobilosti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PosudekRoSkupina",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PosudekRoZpusobilostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkupinaRoPolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosudekRoSkupina", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosudekRoSkupina_PosudkyRoZpusobilosti_PosudekRoZpusobilostId",
                        column: x => x.PosudekRoZpusobilostId,
                        principalTable: "PosudkyRoZpusobilosti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PosudekRoHarmonizovanyKodSkupinaRo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PosudekRoHarmonizovanyKodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkupinaRoPolozkaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosudekRoHarmonizovanyKodSkupinaRo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosudekRoHarmonizovanyKodSkupinaRo_PosudekRoHarmonizovanyKod_PosudekRoHarmonizovanyKodId",
                        column: x => x.PosudekRoHarmonizovanyKodId,
                        principalTable: "PosudekRoHarmonizovanyKod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuthorizedWorkers",
                columns: new[] { "Id", "Ico", "KrzpId" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "12345678", "KRZP-001" });

            migrationBuilder.InsertData(
                table: "Codebooks",
                columns: new[] { "Id", "Kod", "PlatnostDo", "PlatnostOd", "Termx", "TermxId", "TermxUrl", "Verze" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "STAV-POSUDKU", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "1" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "TYP-AKCE", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "1" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "DRUH-PROHLIDKY", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "1" },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "DRUH-POSUDKU", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "1" },
                    { new Guid("20000000-0000-0000-0000-000000000005"), "VYSLEDEK", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "1" },
                    { new Guid("20000000-0000-0000-0000-000000000006"), "SKUPINA-ZADATEL-RIDIC", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "1" },
                    { new Guid("20000000-0000-0000-0000-000000000007"), "SKUPINA-RO", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "1" },
                    { new Guid("20000000-0000-0000-0000-000000000008"), "TYP-OPERACE", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "1" },
                    { new Guid("20000000-0000-0000-0000-000000000009"), "ODBORNOST-LEKARE", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "1" },
                    { new Guid("20000000-0000-0000-0000-00000000000a"), "HARMON-KOD", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "1" },
                    { new Guid("20000000-0000-0000-0000-00000000000b"), "NAROD-KOD", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Id", "CodebookId", "Kod", "RodicId", "Verze" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000001"), "PLATNY", null, "1" },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000001"), "ZNEPLATNENY", null, "1" },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000002"), "VYTVORENI", null, "1" },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000002"), "ZNEPLATNENI", null, "1" },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("20000000-0000-0000-0000-000000000003"), "VSTUPNI", null, "1" },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("20000000-0000-0000-0000-000000000004"), "RO", null, "1" },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("20000000-0000-0000-0000-000000000005"), "ZPUSOBILY", null, "1" },
                    { new Guid("30000000-0000-0000-0000-000000000008"), new Guid("20000000-0000-0000-0000-000000000005"), "NEZPUSOBILY", null, "1" },
                    { new Guid("30000000-0000-0000-0000-000000000009"), new Guid("20000000-0000-0000-0000-000000000006"), "RIDIC", null, "1" },
                    { new Guid("30000000-0000-0000-0000-00000000000a"), new Guid("20000000-0000-0000-0000-000000000007"), "B", null, "1" },
                    { new Guid("30000000-0000-0000-0000-00000000000b"), new Guid("20000000-0000-0000-0000-000000000008"), "CREATE", null, "1" },
                    { new Guid("30000000-0000-0000-0000-00000000000c"), new Guid("20000000-0000-0000-0000-000000000008"), "INVALIDATE", null, "1" },
                    { new Guid("30000000-0000-0000-0000-00000000000d"), new Guid("20000000-0000-0000-0000-000000000009"), "VSEOBECNY", null, "1" },
                    { new Guid("30000000-0000-0000-0000-00000000000e"), new Guid("20000000-0000-0000-0000-00000000000a"), "01", null, "1" },
                    { new Guid("30000000-0000-0000-0000-00000000000f"), new Guid("20000000-0000-0000-0000-00000000000b"), "N01", null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookTranslations",
                columns: new[] { "Id", "CodebookId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000001"), "cs", "Stav posudku", "Stavy posudků." },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000001"), "en", "Certificate state", "Certificate states." },
                    { new Guid("40000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000002"), "cs", "Typ akce", null },
                    { new Guid("40000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000002"), "en", "Action type", null },
                    { new Guid("40000000-0000-0000-0000-000000000005"), new Guid("20000000-0000-0000-0000-000000000003"), "cs", "Druh prohlídky", null },
                    { new Guid("40000000-0000-0000-0000-000000000006"), new Guid("20000000-0000-0000-0000-000000000003"), "en", "Exam type", null },
                    { new Guid("40000000-0000-0000-0000-000000000007"), new Guid("20000000-0000-0000-0000-000000000004"), "cs", "Druh posudku", null },
                    { new Guid("40000000-0000-0000-0000-000000000008"), new Guid("20000000-0000-0000-0000-000000000004"), "en", "Certificate type", null },
                    { new Guid("40000000-0000-0000-0000-000000000009"), new Guid("20000000-0000-0000-0000-000000000005"), "cs", "Výsledek", null },
                    { new Guid("40000000-0000-0000-0000-00000000000a"), new Guid("20000000-0000-0000-0000-000000000005"), "en", "Result", null },
                    { new Guid("40000000-0000-0000-0000-00000000000b"), new Guid("20000000-0000-0000-0000-000000000006"), "cs", "Skupina žadatele", null },
                    { new Guid("40000000-0000-0000-0000-00000000000c"), new Guid("20000000-0000-0000-0000-000000000006"), "en", "Applicant group", null },
                    { new Guid("40000000-0000-0000-0000-00000000000d"), new Guid("20000000-0000-0000-0000-000000000007"), "cs", "Skupiny ŘO", null },
                    { new Guid("40000000-0000-0000-0000-00000000000e"), new Guid("20000000-0000-0000-0000-000000000007"), "en", "Driving licence groups", null },
                    { new Guid("40000000-0000-0000-0000-00000000000f"), new Guid("20000000-0000-0000-0000-000000000008"), "cs", "Typ operace", null },
                    { new Guid("40000000-0000-0000-0000-000000000010"), new Guid("20000000-0000-0000-0000-000000000008"), "en", "Operation type", null },
                    { new Guid("40000000-0000-0000-0000-000000000011"), new Guid("20000000-0000-0000-0000-000000000009"), "cs", "Odbornost", null },
                    { new Guid("40000000-0000-0000-0000-000000000012"), new Guid("20000000-0000-0000-0000-000000000009"), "en", "Specialization", null },
                    { new Guid("40000000-0000-0000-0000-000000000013"), new Guid("20000000-0000-0000-0000-00000000000a"), "cs", "Harmonizované kódy", null },
                    { new Guid("40000000-0000-0000-0000-000000000014"), new Guid("20000000-0000-0000-0000-00000000000a"), "en", "Harmonized codes", null },
                    { new Guid("40000000-0000-0000-0000-000000000015"), new Guid("20000000-0000-0000-0000-00000000000b"), "cs", "Národní kódy", null },
                    { new Guid("40000000-0000-0000-0000-000000000016"), new Guid("20000000-0000-0000-0000-00000000000b"), "en", "National codes", null }
                });

            migrationBuilder.InsertData(
                table: "CodebookItemTranslations",
                columns: new[] { "Id", "CodebookItemId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000001"), "cs", "Platný", null },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("30000000-0000-0000-0000-000000000001"), "en", "Valid", null },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("30000000-0000-0000-0000-000000000002"), "cs", "Zneplatněný", null },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("30000000-0000-0000-0000-000000000002"), "en", "Invalidated", null },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("30000000-0000-0000-0000-000000000003"), "cs", "Vytvoření", null },
                    { new Guid("50000000-0000-0000-0000-000000000006"), new Guid("30000000-0000-0000-0000-000000000003"), "en", "Create", null },
                    { new Guid("50000000-0000-0000-0000-000000000007"), new Guid("30000000-0000-0000-0000-000000000004"), "cs", "Zneplatnění", null },
                    { new Guid("50000000-0000-0000-0000-000000000008"), new Guid("30000000-0000-0000-0000-000000000004"), "en", "Invalidate", null },
                    { new Guid("50000000-0000-0000-0000-000000000009"), new Guid("30000000-0000-0000-0000-000000000005"), "cs", "Vstupní", null },
                    { new Guid("50000000-0000-0000-0000-00000000000a"), new Guid("30000000-0000-0000-0000-000000000005"), "en", "Initial", null },
                    { new Guid("50000000-0000-0000-0000-00000000000b"), new Guid("30000000-0000-0000-0000-000000000006"), "cs", "Řidičské oprávnění", null },
                    { new Guid("50000000-0000-0000-0000-00000000000c"), new Guid("30000000-0000-0000-0000-000000000006"), "en", "Driving licence", null },
                    { new Guid("50000000-0000-0000-0000-00000000000d"), new Guid("30000000-0000-0000-0000-000000000007"), "cs", "Způsobilý", null },
                    { new Guid("50000000-0000-0000-0000-00000000000e"), new Guid("30000000-0000-0000-0000-000000000007"), "en", "Fit", null },
                    { new Guid("50000000-0000-0000-0000-00000000000f"), new Guid("30000000-0000-0000-0000-000000000008"), "cs", "Nezpůsobilý", null },
                    { new Guid("50000000-0000-0000-0000-000000000010"), new Guid("30000000-0000-0000-0000-000000000008"), "en", "Unfit", null },
                    { new Guid("50000000-0000-0000-0000-000000000011"), new Guid("30000000-0000-0000-0000-000000000009"), "cs", "Řidič", null },
                    { new Guid("50000000-0000-0000-0000-000000000012"), new Guid("30000000-0000-0000-0000-000000000009"), "en", "Driver", null },
                    { new Guid("50000000-0000-0000-0000-000000000013"), new Guid("30000000-0000-0000-0000-00000000000a"), "cs", "B", null },
                    { new Guid("50000000-0000-0000-0000-000000000014"), new Guid("30000000-0000-0000-0000-00000000000a"), "en", "B", null },
                    { new Guid("50000000-0000-0000-0000-000000000015"), new Guid("30000000-0000-0000-0000-00000000000b"), "cs", "Vytvoření", null },
                    { new Guid("50000000-0000-0000-0000-000000000016"), new Guid("30000000-0000-0000-0000-00000000000b"), "en", "Create", null },
                    { new Guid("50000000-0000-0000-0000-000000000017"), new Guid("30000000-0000-0000-0000-00000000000c"), "cs", "Zneplatnění", null },
                    { new Guid("50000000-0000-0000-0000-000000000018"), new Guid("30000000-0000-0000-0000-00000000000c"), "en", "Invalidate", null },
                    { new Guid("50000000-0000-0000-0000-000000000019"), new Guid("30000000-0000-0000-0000-00000000000d"), "cs", "Všeobecný lékař", null },
                    { new Guid("50000000-0000-0000-0000-00000000001a"), new Guid("30000000-0000-0000-0000-00000000000d"), "en", "General practitioner", null },
                    { new Guid("50000000-0000-0000-0000-00000000001b"), new Guid("30000000-0000-0000-0000-00000000000e"), "cs", "01", null },
                    { new Guid("50000000-0000-0000-0000-00000000001c"), new Guid("30000000-0000-0000-0000-00000000000e"), "en", "01", null },
                    { new Guid("50000000-0000-0000-0000-00000000001d"), new Guid("30000000-0000-0000-0000-00000000000f"), "cs", "N01", null },
                    { new Guid("50000000-0000-0000-0000-00000000001e"), new Guid("30000000-0000-0000-0000-00000000000f"), "en", "N01", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedWorkers_Ico_KrzpId",
                table: "AuthorizedWorkers",
                columns: new[] { "Ico", "KrzpId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodebookItems_CodebookId",
                table: "CodebookItems",
                column: "CodebookId");

            migrationBuilder.CreateIndex(
                name: "IX_CodebookItemTranslations_CodebookItemId",
                table: "CodebookItemTranslations",
                column: "CodebookItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CodebookTranslations_CodebookId",
                table: "CodebookTranslations",
                column: "CodebookId");

            migrationBuilder.CreateIndex(
                name: "IX_PosudekRoHarmonizovanyKod_PosudekRoZpusobilostId",
                table: "PosudekRoHarmonizovanyKod",
                column: "PosudekRoZpusobilostId");

            migrationBuilder.CreateIndex(
                name: "IX_PosudekRoHarmonizovanyKodSkupinaRo_PosudekRoHarmonizovanyKodId",
                table: "PosudekRoHarmonizovanyKodSkupinaRo",
                column: "PosudekRoHarmonizovanyKodId");

            migrationBuilder.CreateIndex(
                name: "IX_PosudekRoNarodniKod_PosudekRoZpusobilostId",
                table: "PosudekRoNarodniKod",
                column: "PosudekRoZpusobilostId");

            migrationBuilder.CreateIndex(
                name: "IX_PosudekRoSkupina_PosudekRoZpusobilostId",
                table: "PosudekRoSkupina",
                column: "PosudekRoZpusobilostId");

            migrationBuilder.CreateIndex(
                name: "IX_PosudkyRoHistorie_PosudekRoId",
                table: "PosudkyRoHistorie",
                column: "PosudekRoId");

            migrationBuilder.CreateIndex(
                name: "IX_PosudkyRoZpusobilosti_PosudekRoId",
                table: "PosudkyRoZpusobilosti",
                column: "PosudekRoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizedWorkers");

            migrationBuilder.DropTable(
                name: "CodebookItemTranslations");

            migrationBuilder.DropTable(
                name: "CodebookTranslations");

            migrationBuilder.DropTable(
                name: "PosudekRoHarmonizovanyKodSkupinaRo");

            migrationBuilder.DropTable(
                name: "PosudekRoNarodniKod");

            migrationBuilder.DropTable(
                name: "PosudekRoSkupina");

            migrationBuilder.DropTable(
                name: "PosudkyRoHistorie");

            migrationBuilder.DropTable(
                name: "CodebookItems");

            migrationBuilder.DropTable(
                name: "PosudekRoHarmonizovanyKod");

            migrationBuilder.DropTable(
                name: "Codebooks");

            migrationBuilder.DropTable(
                name: "PosudkyRoZpusobilosti");

            migrationBuilder.DropTable(
                name: "PosudkyRo");
        }
    }
}
