using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicMedicalCert.Infrastructure.Data.Migrations
{
    /// <summary>
    /// Renames existing codelist item codes from short names to TermX naming convention,
    /// and inserts new codelist items that had no old equivalent.
    /// </summary>
    public partial class Seed_Termx_Remaining_Codelists : Migration
    {
        // Existing item IDs (from initial seed - 30000000-*)
        private static readonly Guid ItStavPlatny = Guid.Parse("30000000-0000-0000-0000-000000000001");
        private static readonly Guid ItStavZneplatneny = Guid.Parse("30000000-0000-0000-0000-000000000002");
        private static readonly Guid ItAkceVytvoreni = Guid.Parse("30000000-0000-0000-0000-000000000003");
        private static readonly Guid ItAkceZneplatneni = Guid.Parse("30000000-0000-0000-0000-000000000004");
        private static readonly Guid ItDruhProhlidkyVstupni = Guid.Parse("30000000-0000-0000-0000-000000000005");
        private static readonly Guid ItDruhPosudkuRo = Guid.Parse("30000000-0000-0000-0000-000000000006");
        private static readonly Guid ItVysledekZpusobily = Guid.Parse("30000000-0000-0000-0000-000000000007");
        private static readonly Guid ItVysledekNezpusobily = Guid.Parse("30000000-0000-0000-0000-000000000008");
        private static readonly Guid ItSkupinaZadatelRidic = Guid.Parse("30000000-0000-0000-0000-000000000009");

        // Codebook IDs
        private static readonly Guid CbDruhProhlidky = Guid.Parse("20000000-0000-0000-0000-000000000003");
        private static readonly Guid CbVysledek = Guid.Parse("20000000-0000-0000-0000-000000000005");
        private static readonly Guid CbSkupinaZadateleRidic = Guid.Parse("20000000-0000-0000-0000-000000000006");

        // New item IDs
        private static readonly Guid ItDruhProhlidkyPeriodicka = Guid.Parse("80000000-0000-0000-0000-000000000006");
        private static readonly Guid ItDruhProhlidkyMimoradna = Guid.Parse("80000000-0000-0000-0000-000000000007");
        private static readonly Guid ItVysledekZpusobilySPodminkou = Guid.Parse("80000000-0000-0000-0000-00000000000B");
        private static readonly Guid ItSkupinaZadatelRidic2 = Guid.Parse("80000000-0000-0000-0000-00000000000D");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ===================================================================
            // RENAME existing codelist item codes to TermX convention
            // ===================================================================

            // STAV-POSUDKU
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItStavPlatny,
                column: "Kod", value: "stav_posudku_platny");
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItStavZneplatneny,
                column: "Kod", value: "stav_posudku_zneplatneny");

            // TYP-AKCE
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItAkceVytvoreni,
                column: "Kod", value: "akce_ro_vytvoreni");
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItAkceZneplatneni,
                column: "Kod", value: "akce_ro_zneplatneni");

            // DRUH-PROHLIDKY
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItDruhProhlidkyVstupni,
                column: "Kod", value: "druh_prohlidky_ro_vstupni");

            // DRUH-POSUDKU
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItDruhPosudkuRo,
                column: "Kod", value: "druh_posudku_ro");

            // VYSLEDEK
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItVysledekZpusobily,
                column: "Kod", value: "vysledek_posudku_ro_zpusobily");
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItVysledekNezpusobily,
                column: "Kod", value: "vysledek_posudku_ro_nezpusobily");

            // SKUPINA-ZADATEL-RIDIC
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItSkupinaZadatelRidic,
                column: "Kod", value: "skupina_ro_1");

            // ===================================================================
            // UPDATE translations for renamed items
            // ===================================================================

            // TYP-AKCE: en translations
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-000000000006"),
                column: "Nazev", value: "Creation");
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-000000000008"),
                column: "Nazev", value: "Invalidation");

            // DRUH-PROHLIDKY translations
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-000000000009"),
                column: "Nazev", value: "Vstupní prohlídka");
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-00000000000A"),
                column: "Nazev", value: "Initial examination");

            // SKUPINA-ZADATEL-RIDIC translations
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-000000000011"),
                column: "Nazev", value: "Skupina 1");
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-000000000012"),
                column: "Nazev", value: "Group 1");

            // ===================================================================
            // INSERT new codelist items (no old equivalent)
            // ===================================================================

            // DRUH-PROHLIDKY: periodicka, mimoradna
            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Id", "CodebookId", "Kod", "RodicId", "Verze" },
                values: new object[,]
                {
                    { ItDruhProhlidkyPeriodicka, CbDruhProhlidky, "druh_prohlidky_ro_periodicka", null, "1" },
                    { ItDruhProhlidkyMimoradna, CbDruhProhlidky, "druh_prohlidky_ro_mimoradna", null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItemTranslations",
                columns: new[] { "Id", "CodebookItemId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { Guid.Parse("90000000-0000-0000-0000-00000000000B"), ItDruhProhlidkyPeriodicka, "cs", "Periodická prohlídka", null },
                    { Guid.Parse("90000000-0000-0000-0000-00000000000C"), ItDruhProhlidkyPeriodicka, "en", "Periodic examination", null },
                    { Guid.Parse("90000000-0000-0000-0000-00000000000D"), ItDruhProhlidkyMimoradna, "cs", "Mimořádná prohlídka", null },
                    { Guid.Parse("90000000-0000-0000-0000-00000000000E"), ItDruhProhlidkyMimoradna, "en", "Extraordinary examination", null }
                });

            // VYSLEDEK: zpusobily_s_podminkou
            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Id", "CodebookId", "Kod", "RodicId", "Verze" },
                values: new object[,]
                {
                    { ItVysledekZpusobilySPodminkou, CbVysledek, "vysledek_posudku_ro_zpusobily_s_podminkou", null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItemTranslations",
                columns: new[] { "Id", "CodebookItemId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { Guid.Parse("90000000-0000-0000-0000-000000000015"), ItVysledekZpusobilySPodminkou, "cs", "Způsobilý s podmínkou", null },
                    { Guid.Parse("90000000-0000-0000-0000-000000000016"), ItVysledekZpusobilySPodminkou, "en", "Fit with condition", null }
                });

            // SKUPINA-ZADATEL-RIDIC: skupina_ro_2
            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Id", "CodebookId", "Kod", "RodicId", "Verze" },
                values: new object[,]
                {
                    { ItSkupinaZadatelRidic2, CbSkupinaZadateleRidic, "skupina_ro_2", null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItemTranslations",
                columns: new[] { "Id", "CodebookItemId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { Guid.Parse("90000000-0000-0000-0000-000000000019"), ItSkupinaZadatelRidic2, "cs", "Skupina 2", null },
                    { Guid.Parse("90000000-0000-0000-0000-00000000001A"), ItSkupinaZadatelRidic2, "en", "Group 2", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete new translations
            foreach (var id in new[]
            {
                Guid.Parse("90000000-0000-0000-0000-00000000000B"),
                Guid.Parse("90000000-0000-0000-0000-00000000000C"),
                Guid.Parse("90000000-0000-0000-0000-00000000000D"),
                Guid.Parse("90000000-0000-0000-0000-00000000000E"),
                Guid.Parse("90000000-0000-0000-0000-000000000015"),
                Guid.Parse("90000000-0000-0000-0000-000000000016"),
                Guid.Parse("90000000-0000-0000-0000-000000000019"),
                Guid.Parse("90000000-0000-0000-0000-00000000001A")
            })
            {
                migrationBuilder.DeleteData(table: "CodebookItemTranslations", keyColumn: "Id", keyValue: id);
            }

            // Delete new items
            foreach (var id in new[] { ItDruhProhlidkyPeriodicka, ItDruhProhlidkyMimoradna, ItVysledekZpusobilySPodminkou, ItSkupinaZadatelRidic2 })
            {
                migrationBuilder.DeleteData(table: "CodebookItems", keyColumn: "Id", keyValue: id);
            }

            // Revert renamed codes back to old names
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItStavPlatny, column: "Kod", value: "PLATNY");
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItStavZneplatneny, column: "Kod", value: "ZNEPLATNENY");
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItAkceVytvoreni, column: "Kod", value: "VYTVORENI");
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItAkceZneplatneni, column: "Kod", value: "ZNEPLATNENI");
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItDruhProhlidkyVstupni, column: "Kod", value: "VSTUPNI");
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItDruhPosudkuRo, column: "Kod", value: "RO");
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItVysledekZpusobily, column: "Kod", value: "ZPUSOBILY");
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItVysledekNezpusobily, column: "Kod", value: "NEZPUSOBILY");
            migrationBuilder.UpdateData(table: "CodebookItems", keyColumn: "Id", keyValue: ItSkupinaZadatelRidic, column: "Kod", value: "RIDIC");

            // Revert translations
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-000000000006"), column: "Nazev", value: "Create");
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-000000000008"), column: "Nazev", value: "Invalidate");
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-000000000009"), column: "Nazev", value: "Vstupní");
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-00000000000A"), column: "Nazev", value: "Initial");
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-000000000011"), column: "Nazev", value: "Řidič");
            migrationBuilder.UpdateData(table: "CodebookItemTranslations",
                keyColumn: "Id", keyValue: Guid.Parse("50000000-0000-0000-0000-000000000012"), column: "Nazev", value: "Driver");
        }
    }
}

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ===================================================================
            // STAV-POSUDKU (elp-ro-stav-posudku, filter: stav_posudku_.*)
            // ===================================================================
            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Id", "CodebookId", "Kod", "RodicId", "Verze" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000001"), CbStavPosudku, "stav_posudku_platny", null, "1" },
                    { new Guid("80000000-0000-0000-0000-000000000002"), CbStavPosudku, "stav_posudku_zneplatneny", null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItemTranslations",
                columns: new[] { "Id", "CodebookItemId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000001"), new Guid("80000000-0000-0000-0000-000000000001"), "cs", "Platný", null },
                    { new Guid("90000000-0000-0000-0000-000000000002"), new Guid("80000000-0000-0000-0000-000000000001"), "en", "Valid", null },
                    { new Guid("90000000-0000-0000-0000-000000000003"), new Guid("80000000-0000-0000-0000-000000000002"), "cs", "Zneplatněný", null },
                    { new Guid("90000000-0000-0000-0000-000000000004"), new Guid("80000000-0000-0000-0000-000000000002"), "en", "Invalidated", null }
                });

            // ===================================================================
            // TYP-AKCE (elp-ro-akce, filter: akce_ro_.*)
            // ===================================================================
            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Id", "CodebookId", "Kod", "RodicId", "Verze" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000003"), CbTypAkce, "akce_ro_vytvoreni", null, "1" },
                    { new Guid("80000000-0000-0000-0000-000000000004"), CbTypAkce, "akce_ro_zneplatneni", null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItemTranslations",
                columns: new[] { "Id", "CodebookItemId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000005"), new Guid("80000000-0000-0000-0000-000000000003"), "cs", "Vytvoření", null },
                    { new Guid("90000000-0000-0000-0000-000000000006"), new Guid("80000000-0000-0000-0000-000000000003"), "en", "Creation", null },
                    { new Guid("90000000-0000-0000-0000-000000000007"), new Guid("80000000-0000-0000-0000-000000000004"), "cs", "Zneplatnění", null },
                    { new Guid("90000000-0000-0000-0000-000000000008"), new Guid("80000000-0000-0000-0000-000000000004"), "en", "Invalidation", null }
                });

            // ===================================================================
            // DRUH-PROHLIDKY (elp-ro-druh-prohlidky)
            // ===================================================================
            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Id", "CodebookId", "Kod", "RodicId", "Verze" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000005"), CbDruhProhlidky, "druh_prohlidky_ro_vstupni", null, "1" },
                    { new Guid("80000000-0000-0000-0000-000000000006"), CbDruhProhlidky, "druh_prohlidky_ro_periodicka", null, "1" },
                    { new Guid("80000000-0000-0000-0000-000000000007"), CbDruhProhlidky, "druh_prohlidky_ro_mimoradna", null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItemTranslations",
                columns: new[] { "Id", "CodebookItemId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000009"), new Guid("80000000-0000-0000-0000-000000000005"), "cs", "Vstupní prohlídka", null },
                    { new Guid("90000000-0000-0000-0000-00000000000A"), new Guid("80000000-0000-0000-0000-000000000005"), "en", "Initial examination", null },
                    { new Guid("90000000-0000-0000-0000-00000000000B"), new Guid("80000000-0000-0000-0000-000000000006"), "cs", "Periodická prohlídka", null },
                    { new Guid("90000000-0000-0000-0000-00000000000C"), new Guid("80000000-0000-0000-0000-000000000006"), "en", "Periodic examination", null },
                    { new Guid("90000000-0000-0000-0000-00000000000D"), new Guid("80000000-0000-0000-0000-000000000007"), "cs", "Mimořádná prohlídka", null },
                    { new Guid("90000000-0000-0000-0000-00000000000E"), new Guid("80000000-0000-0000-0000-000000000007"), "en", "Extraordinary examination", null }
                });

            // ===================================================================
            // DRUH-POSUDKU (elp-ro-druh-posudku, filter: druh_posudku_ro_.*)
            // ===================================================================
            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Id", "CodebookId", "Kod", "RodicId", "Verze" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000008"), CbDruhPosudku, "druh_posudku_ro", null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItemTranslations",
                columns: new[] { "Id", "CodebookItemId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-00000000000F"), new Guid("80000000-0000-0000-0000-000000000008"), "cs", "Řidičské oprávnění", null },
                    { new Guid("90000000-0000-0000-0000-000000000010"), new Guid("80000000-0000-0000-0000-000000000008"), "en", "Driving licence", null }
                });

            // ===================================================================
            // VYSLEDEK (elp-ro-vysledek-posudku, filter: vysledek_posudku_ro_.*)
            // ===================================================================
            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Id", "CodebookId", "Kod", "RodicId", "Verze" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000009"), CbVysledek, "vysledek_posudku_ro_zpusobily", null, "1" },
                    { new Guid("80000000-0000-0000-0000-00000000000A"), CbVysledek, "vysledek_posudku_ro_nezpusobily", null, "1" },
                    { new Guid("80000000-0000-0000-0000-00000000000B"), CbVysledek, "vysledek_posudku_ro_zpusobily_s_podminkou", null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItemTranslations",
                columns: new[] { "Id", "CodebookItemId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000011"), new Guid("80000000-0000-0000-0000-000000000009"), "cs", "Způsobilý", null },
                    { new Guid("90000000-0000-0000-0000-000000000012"), new Guid("80000000-0000-0000-0000-000000000009"), "en", "Fit", null },
                    { new Guid("90000000-0000-0000-0000-000000000013"), new Guid("80000000-0000-0000-0000-00000000000A"), "cs", "Nezpůsobilý", null },
                    { new Guid("90000000-0000-0000-0000-000000000014"), new Guid("80000000-0000-0000-0000-00000000000A"), "en", "Unfit", null },
                    { new Guid("90000000-0000-0000-0000-000000000015"), new Guid("80000000-0000-0000-0000-00000000000B"), "cs", "Způsobilý s podmínkou", null },
                    { new Guid("90000000-0000-0000-0000-000000000016"), new Guid("80000000-0000-0000-0000-00000000000B"), "en", "Fit with condition", null }
                });

            // ===================================================================
            // SKUPINA-ZADATEL-RIDIC (elp-ro-zadatel-skupina, filter: skupina_ro_.*)
            // ===================================================================
            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Id", "CodebookId", "Kod", "RodicId", "Verze" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-00000000000C"), CbSkupinaZadateleRidic, "skupina_ro_1", null, "1" },
                    { new Guid("80000000-0000-0000-0000-00000000000D"), CbSkupinaZadateleRidic, "skupina_ro_2", null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItemTranslations",
                columns: new[] { "Id", "CodebookItemId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000017"), new Guid("80000000-0000-0000-0000-00000000000C"), "cs", "Skupina 1", null },
                    { new Guid("90000000-0000-0000-0000-000000000018"), new Guid("80000000-0000-0000-0000-00000000000C"), "en", "Group 1", null },
                    { new Guid("90000000-0000-0000-0000-000000000019"), new Guid("80000000-0000-0000-0000-00000000000D"), "cs", "Skupina 2", null },
                    { new Guid("90000000-0000-0000-0000-00000000001A"), new Guid("80000000-0000-0000-0000-00000000000D"), "en", "Group 2", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete translations first (FK constraint)
            var translationIds = new[]
            {
                new Guid("90000000-0000-0000-0000-000000000001"),
                new Guid("90000000-0000-0000-0000-000000000002"),
                new Guid("90000000-0000-0000-0000-000000000003"),
                new Guid("90000000-0000-0000-0000-000000000004"),
                new Guid("90000000-0000-0000-0000-000000000005"),
                new Guid("90000000-0000-0000-0000-000000000006"),
                new Guid("90000000-0000-0000-0000-000000000007"),
                new Guid("90000000-0000-0000-0000-000000000008"),
                new Guid("90000000-0000-0000-0000-000000000009"),
                new Guid("90000000-0000-0000-0000-00000000000A"),
                new Guid("90000000-0000-0000-0000-00000000000B"),
                new Guid("90000000-0000-0000-0000-00000000000C"),
                new Guid("90000000-0000-0000-0000-00000000000D"),
                new Guid("90000000-0000-0000-0000-00000000000E"),
                new Guid("90000000-0000-0000-0000-00000000000F"),
                new Guid("90000000-0000-0000-0000-000000000010"),
                new Guid("90000000-0000-0000-0000-000000000011"),
                new Guid("90000000-0000-0000-0000-000000000012"),
                new Guid("90000000-0000-0000-0000-000000000013"),
                new Guid("90000000-0000-0000-0000-000000000014"),
                new Guid("90000000-0000-0000-0000-000000000015"),
                new Guid("90000000-0000-0000-0000-000000000016"),
                new Guid("90000000-0000-0000-0000-000000000017"),
                new Guid("90000000-0000-0000-0000-000000000018"),
                new Guid("90000000-0000-0000-0000-000000000019"),
                new Guid("90000000-0000-0000-0000-00000000001A")
            };

            foreach (var id in translationIds)
            {
                migrationBuilder.DeleteData(table: "CodebookItemTranslations", keyColumn: "Id", keyValue: id);
            }

            // Delete codebook items
            var itemIds = new[]
            {
                new Guid("80000000-0000-0000-0000-000000000001"),
                new Guid("80000000-0000-0000-0000-000000000002"),
                new Guid("80000000-0000-0000-0000-000000000003"),
                new Guid("80000000-0000-0000-0000-000000000004"),
                new Guid("80000000-0000-0000-0000-000000000005"),
                new Guid("80000000-0000-0000-0000-000000000006"),
                new Guid("80000000-0000-0000-0000-000000000007"),
                new Guid("80000000-0000-0000-0000-000000000008"),
                new Guid("80000000-0000-0000-0000-000000000009"),
                new Guid("80000000-0000-0000-0000-00000000000A"),
                new Guid("80000000-0000-0000-0000-00000000000B"),
                new Guid("80000000-0000-0000-0000-00000000000C"),
                new Guid("80000000-0000-0000-0000-00000000000D")
            };

            foreach (var id in itemIds)
            {
                migrationBuilder.DeleteData(table: "CodebookItems", keyColumn: "Id", keyValue: id);
            }
        }
    }
}
