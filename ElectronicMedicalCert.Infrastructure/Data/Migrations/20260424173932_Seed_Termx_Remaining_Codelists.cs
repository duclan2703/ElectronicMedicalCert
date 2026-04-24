using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ElectronicMedicalCert.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Termx_Remaining_Codelists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000006"),
                column: "Nazev",
                value: "Creation");

            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000008"),
                column: "Nazev",
                value: "Invalidation");

            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000009"),
                column: "Nazev",
                value: "Vstupní prohlídka");

            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-00000000000a"),
                column: "Nazev",
                value: "Initial examination");

            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000011"),
                column: "Nazev",
                value: "Skupina 1");

            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000012"),
                column: "Nazev",
                value: "Group 1");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                column: "Kod",
                value: "stav_posudku_platny");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"),
                column: "Kod",
                value: "stav_posudku_zneplatneny");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"),
                column: "Kod",
                value: "akce_ro_vytvoreni");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"),
                column: "Kod",
                value: "akce_ro_zneplatneni");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000005"),
                column: "Kod",
                value: "druh_prohlidky_ro_vstupni");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000006"),
                column: "Kod",
                value: "druh_posudku_ro");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000007"),
                column: "Kod",
                value: "vysledek_posudku_ro_zpusobily");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000008"),
                column: "Kod",
                value: "vysledek_posudku_ro_nezpusobily");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000009"),
                column: "Kod",
                value: "skupina_ro_1");

            migrationBuilder.InsertData(
                table: "CodebookItems",
                columns: new[] { "Id", "CodebookId", "Kod", "RodicId", "Verze" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000006"), new Guid("20000000-0000-0000-0000-000000000003"), "druh_prohlidky_ro_periodicka", null, "1" },
                    { new Guid("80000000-0000-0000-0000-000000000007"), new Guid("20000000-0000-0000-0000-000000000003"), "druh_prohlidky_ro_mimoradna", null, "1" },
                    { new Guid("80000000-0000-0000-0000-00000000000b"), new Guid("20000000-0000-0000-0000-000000000005"), "vysledek_posudku_ro_zpusobily_s_podminkou", null, "1" },
                    { new Guid("80000000-0000-0000-0000-00000000000d"), new Guid("20000000-0000-0000-0000-000000000006"), "skupina_ro_2", null, "1" }
                });

            migrationBuilder.InsertData(
                table: "CodebookItemTranslations",
                columns: new[] { "Id", "CodebookItemId", "Language", "Nazev", "Popis" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-00000000000b"), new Guid("80000000-0000-0000-0000-000000000006"), "cs", "Periodická prohlídka", null },
                    { new Guid("90000000-0000-0000-0000-00000000000c"), new Guid("80000000-0000-0000-0000-000000000006"), "en", "Periodic examination", null },
                    { new Guid("90000000-0000-0000-0000-00000000000d"), new Guid("80000000-0000-0000-0000-000000000007"), "cs", "Mimořádná prohlídka", null },
                    { new Guid("90000000-0000-0000-0000-00000000000e"), new Guid("80000000-0000-0000-0000-000000000007"), "en", "Extraordinary examination", null },
                    { new Guid("90000000-0000-0000-0000-000000000015"), new Guid("80000000-0000-0000-0000-00000000000b"), "cs", "Způsobilý s podmínkou", null },
                    { new Guid("90000000-0000-0000-0000-000000000016"), new Guid("80000000-0000-0000-0000-00000000000b"), "en", "Fit with condition", null },
                    { new Guid("90000000-0000-0000-0000-000000000019"), new Guid("80000000-0000-0000-0000-00000000000d"), "cs", "Skupina 2", null },
                    { new Guid("90000000-0000-0000-0000-00000000001a"), new Guid("80000000-0000-0000-0000-00000000000d"), "en", "Group 2", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("90000000-0000-0000-0000-00000000000b"));

            migrationBuilder.DeleteData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("90000000-0000-0000-0000-00000000000c"));

            migrationBuilder.DeleteData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("90000000-0000-0000-0000-00000000000d"));

            migrationBuilder.DeleteData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("90000000-0000-0000-0000-00000000000e"));

            migrationBuilder.DeleteData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("90000000-0000-0000-0000-00000000001a"));

            migrationBuilder.DeleteData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-00000000000b"));

            migrationBuilder.DeleteData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-00000000000d"));

            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000006"),
                column: "Nazev",
                value: "Create");

            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000008"),
                column: "Nazev",
                value: "Invalidate");

            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000009"),
                column: "Nazev",
                value: "Vstupní");

            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-00000000000a"),
                column: "Nazev",
                value: "Initial");

            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000011"),
                column: "Nazev",
                value: "Řidič");

            migrationBuilder.UpdateData(
                table: "CodebookItemTranslations",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000012"),
                column: "Nazev",
                value: "Driver");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                column: "Kod",
                value: "PLATNY");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"),
                column: "Kod",
                value: "ZNEPLATNENY");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"),
                column: "Kod",
                value: "VYTVORENI");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"),
                column: "Kod",
                value: "ZNEPLATNENI");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000005"),
                column: "Kod",
                value: "VSTUPNI");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000006"),
                column: "Kod",
                value: "RO");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000007"),
                column: "Kod",
                value: "ZPUSOBILY");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000008"),
                column: "Kod",
                value: "NEZPUSOBILY");

            migrationBuilder.UpdateData(
                table: "CodebookItems",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000009"),
                column: "Kod",
                value: "RIDIC");
        }
    }
}
