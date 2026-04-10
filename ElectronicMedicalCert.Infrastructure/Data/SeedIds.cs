namespace ElectronicMedicalCert.Infrastructure.Data;

internal static class SeedIds
{
    public static readonly DateTime ValidFrom = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static readonly Guid AuthorizedWorker1 = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static readonly Guid CbStavPosudku = Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid CbTypAkce = Guid.Parse("20000000-0000-0000-0000-000000000002");
    public static readonly Guid CbDruhProhlidky = Guid.Parse("20000000-0000-0000-0000-000000000003");
    public static readonly Guid CbDruhPosudku = Guid.Parse("20000000-0000-0000-0000-000000000004");
    public static readonly Guid CbVysledek = Guid.Parse("20000000-0000-0000-0000-000000000005");
    public static readonly Guid CbSkupinaZadateleRidic = Guid.Parse("20000000-0000-0000-0000-000000000006");
    public static readonly Guid CbSkupinaRo = Guid.Parse("20000000-0000-0000-0000-000000000007");
    public static readonly Guid CbTypOperace = Guid.Parse("20000000-0000-0000-0000-000000000008");
    public static readonly Guid CbOdbornost = Guid.Parse("20000000-0000-0000-0000-000000000009");
    public static readonly Guid CbHarmonKod = Guid.Parse("20000000-0000-0000-0000-00000000000A");
    public static readonly Guid CbNarodKod = Guid.Parse("20000000-0000-0000-0000-00000000000B");

    public static readonly Guid ItStavPlatny = Guid.Parse("30000000-0000-0000-0000-000000000001");
    public static readonly Guid ItStavZneplatneny = Guid.Parse("30000000-0000-0000-0000-000000000002");
    public static readonly Guid ItAkceVytvoreni = Guid.Parse("30000000-0000-0000-0000-000000000003");
    public static readonly Guid ItAkceZneplatneni = Guid.Parse("30000000-0000-0000-0000-000000000004");
    public static readonly Guid ItDruhProhlidkyVstupni = Guid.Parse("30000000-0000-0000-0000-000000000005");
    public static readonly Guid ItDruhPosudkuRo = Guid.Parse("30000000-0000-0000-0000-000000000006");
    public static readonly Guid ItVysledekZpusobily = Guid.Parse("30000000-0000-0000-0000-000000000007");
    public static readonly Guid ItVysledekNezpusobily = Guid.Parse("30000000-0000-0000-0000-000000000008");
    public static readonly Guid ItSkupinaZadatelRidic = Guid.Parse("30000000-0000-0000-0000-000000000009");
    public static readonly Guid ItSkupinaRoB = Guid.Parse("30000000-0000-0000-0000-00000000000A");
    public static readonly Guid ItOperaceCreate = Guid.Parse("30000000-0000-0000-0000-00000000000B");
    public static readonly Guid ItOperaceInvalidate = Guid.Parse("30000000-0000-0000-0000-00000000000C");
    public static readonly Guid ItOdbornostVseobecny = Guid.Parse("30000000-0000-0000-0000-00000000000D");
    public static readonly Guid ItHarmon01 = Guid.Parse("30000000-0000-0000-0000-00000000000E");
    public static readonly Guid ItNarod01 = Guid.Parse("30000000-0000-0000-0000-00000000000F");

    public static readonly Guid CbStavPosudkuTrCs = Guid.Parse("40000000-0000-0000-0000-000000000001");
    public static readonly Guid CbStavPosudkuTrEn = Guid.Parse("40000000-0000-0000-0000-000000000002");
    public static readonly Guid CbTypAkceTrCs = Guid.Parse("40000000-0000-0000-0000-000000000003");
    public static readonly Guid CbTypAkceTrEn = Guid.Parse("40000000-0000-0000-0000-000000000004");
    public static readonly Guid CbDruhProhlidkyTrCs = Guid.Parse("40000000-0000-0000-0000-000000000005");
    public static readonly Guid CbDruhProhlidkyTrEn = Guid.Parse("40000000-0000-0000-0000-000000000006");
    public static readonly Guid CbDruhPosudkuTrCs = Guid.Parse("40000000-0000-0000-0000-000000000007");
    public static readonly Guid CbDruhPosudkuTrEn = Guid.Parse("40000000-0000-0000-0000-000000000008");
    public static readonly Guid CbVysledekTrCs = Guid.Parse("40000000-0000-0000-0000-000000000009");
    public static readonly Guid CbVysledekTrEn = Guid.Parse("40000000-0000-0000-0000-00000000000A");
    public static readonly Guid CbSkupinaZadateleRidicTrCs = Guid.Parse("40000000-0000-0000-0000-00000000000B");
    public static readonly Guid CbSkupinaZadateleRidicTrEn = Guid.Parse("40000000-0000-0000-0000-00000000000C");
    public static readonly Guid CbSkupinaRoTrCs = Guid.Parse("40000000-0000-0000-0000-00000000000D");
    public static readonly Guid CbSkupinaRoTrEn = Guid.Parse("40000000-0000-0000-0000-00000000000E");
    public static readonly Guid CbTypOperaceTrCs = Guid.Parse("40000000-0000-0000-0000-00000000000F");
    public static readonly Guid CbTypOperaceTrEn = Guid.Parse("40000000-0000-0000-0000-000000000010");
    public static readonly Guid CbOdbornostTrCs = Guid.Parse("40000000-0000-0000-0000-000000000011");
    public static readonly Guid CbOdbornostTrEn = Guid.Parse("40000000-0000-0000-0000-000000000012");
    public static readonly Guid CbHarmonKodTrCs = Guid.Parse("40000000-0000-0000-0000-000000000013");
    public static readonly Guid CbHarmonKodTrEn = Guid.Parse("40000000-0000-0000-0000-000000000014");
    public static readonly Guid CbNarodKodTrCs = Guid.Parse("40000000-0000-0000-0000-000000000015");
    public static readonly Guid CbNarodKodTrEn = Guid.Parse("40000000-0000-0000-0000-000000000016");

    public static readonly Guid ItStavPlatnyTrCs = Guid.Parse("50000000-0000-0000-0000-000000000001");
    public static readonly Guid ItStavPlatnyTrEn = Guid.Parse("50000000-0000-0000-0000-000000000002");
    public static readonly Guid ItStavZneplatnenyTrCs = Guid.Parse("50000000-0000-0000-0000-000000000003");
    public static readonly Guid ItStavZneplatnenyTrEn = Guid.Parse("50000000-0000-0000-0000-000000000004");
    public static readonly Guid ItAkceVytvoreniTrCs = Guid.Parse("50000000-0000-0000-0000-000000000005");
    public static readonly Guid ItAkceVytvoreniTrEn = Guid.Parse("50000000-0000-0000-0000-000000000006");
    public static readonly Guid ItAkceZneplatneniTrCs = Guid.Parse("50000000-0000-0000-0000-000000000007");
    public static readonly Guid ItAkceZneplatneniTrEn = Guid.Parse("50000000-0000-0000-0000-000000000008");
    public static readonly Guid ItDruhProhlidkyVstupniTrCs = Guid.Parse("50000000-0000-0000-0000-000000000009");
    public static readonly Guid ItDruhProhlidkyVstupniTrEn = Guid.Parse("50000000-0000-0000-0000-00000000000A");
    public static readonly Guid ItDruhPosudkuRoTrCs = Guid.Parse("50000000-0000-0000-0000-00000000000B");
    public static readonly Guid ItDruhPosudkuRoTrEn = Guid.Parse("50000000-0000-0000-0000-00000000000C");
    public static readonly Guid ItVysledekZpusobilyTrCs = Guid.Parse("50000000-0000-0000-0000-00000000000D");
    public static readonly Guid ItVysledekZpusobilyTrEn = Guid.Parse("50000000-0000-0000-0000-00000000000E");
    public static readonly Guid ItVysledekNezpusobilyTrCs = Guid.Parse("50000000-0000-0000-0000-00000000000F");
    public static readonly Guid ItVysledekNezpusobilyTrEn = Guid.Parse("50000000-0000-0000-0000-000000000010");
    public static readonly Guid ItSkupinaZadatelRidicTrCs = Guid.Parse("50000000-0000-0000-0000-000000000011");
    public static readonly Guid ItSkupinaZadatelRidicTrEn = Guid.Parse("50000000-0000-0000-0000-000000000012");
    public static readonly Guid ItSkupinaRoBTrCs = Guid.Parse("50000000-0000-0000-0000-000000000013");
    public static readonly Guid ItSkupinaRoBTrEn = Guid.Parse("50000000-0000-0000-0000-000000000014");
    public static readonly Guid ItOperaceCreateTrCs = Guid.Parse("50000000-0000-0000-0000-000000000015");
    public static readonly Guid ItOperaceCreateTrEn = Guid.Parse("50000000-0000-0000-0000-000000000016");
    public static readonly Guid ItOperaceInvalidateTrCs = Guid.Parse("50000000-0000-0000-0000-000000000017");
    public static readonly Guid ItOperaceInvalidateTrEn = Guid.Parse("50000000-0000-0000-0000-000000000018");
    public static readonly Guid ItOdbornostVseobecnyTrCs = Guid.Parse("50000000-0000-0000-0000-000000000019");
    public static readonly Guid ItOdbornostVseobecnyTrEn = Guid.Parse("50000000-0000-0000-0000-00000000001A");
    public static readonly Guid ItHarmon01TrCs = Guid.Parse("50000000-0000-0000-0000-00000000001B");
    public static readonly Guid ItHarmon01TrEn = Guid.Parse("50000000-0000-0000-0000-00000000001C");
    public static readonly Guid ItNarod01TrCs = Guid.Parse("50000000-0000-0000-0000-00000000001D");
    public static readonly Guid ItNarod01TrEn = Guid.Parse("50000000-0000-0000-0000-00000000001E");
}

