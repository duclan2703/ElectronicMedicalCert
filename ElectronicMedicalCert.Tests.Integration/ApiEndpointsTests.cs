using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ElectronicMedicalCert.Application.Contracts.V2;
using FluentAssertions;
using Xunit;

namespace ElectronicMedicalCert.Tests.Integration;

public sealed class ApiEndpointsTests : IClassFixture<TestWebApplicationFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly TestWebApplicationFactory _factory;

    public ApiEndpointsTests(TestWebApplicationFactory factory) => _factory = factory;

    [Fact]
    public async Task GetCodebooks_requires_auth_and_returns_problem_details()
    {
        var client = _factory.CreateClient();

        var resp = await client.GetAsync("/elektronickePosudky/api/v2/ciselniky");

        resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        resp.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");
        resp.Headers.Should().ContainKey("X-Correlation-Id");

        var body = await resp.Content.ReadAsStringAsync();
        body.Should().Contain("\"type\":\"https://api.elp.cz/errors/unauthorized\"");
    }

    [Fact]
    public async Task Full_flow_create_get_invalidate_history_pdf_search()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = TestWebApplicationFactory.CreateAuthHeader();

        var create = new PosudekRoCreateDto
        {
            Rid = "RID-123",
            KrzpId = "KRZP-001",
            TypAkce = new PosudekRoCiselnikPolozkaCreateDto { Kod = "VYTVORENI", Verze = "1" },
            StavPosudku = new PosudekRoCiselnikPolozkaCreateDto { Kod = "PLATNY", Verze = "1" },
            DruhProhlidky = new PosudekRoCiselnikPolozkaCreateDto { Kod = "VSTUPNI", Verze = "1" },
            DruhPosudku = new PosudekRoCiselnikPolozkaCreateDto { Kod = "RO", Verze = "1" },
            DatumVystaveni = new DateOnly(2026, 1, 1),
            Zpusobilosti =
            {
                new PosudekRoZpusobilostCreateDto
                {
                    SkupinaZadateleRidic = new PosudekRoCiselnikPolozkaCreateDto { Kod = "RIDIC", Verze = "1" },
                    Vysledek = new PosudekRoCiselnikPolozkaCreateDto { Kod = "ZPUSOBILY", Verze = "1" },
                    SkupinyRidicskehoOpravneni =
                    {
                        new PosudekRoSkupinaCreateDto { SkupinaRo = new PosudekRoCiselnikPolozkaCreateDto { Kod = "B", Verze = "1" } }
                    }
                }
            }
        };

        var createResp = await client.PostAsJsonAsync("/elektronickePosudky/api/v2/posudky/ridicskeOpravneni", create, JsonOptions);
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResp.Content.ReadFromJsonAsync<PosudekRoCreateResultDto>(JsonOptions);
        created.Should().NotBeNull();
        created!.Zpusobilosti.Should().NotBeNull();
        created.Zpusobilosti!.Should().NotBeEmpty();
        var id = created.Zpusobilosti[0].PosudekId;

        var detailResp = await client.GetAsync($"/elektronickePosudky/api/v2/posudky/ridicskeOpravneni/{id}");
        detailResp.StatusCode.Should().Be(HttpStatusCode.OK);
        detailResp.Headers.TryGetValues("ETag", out var etagValues).Should().BeTrue();
        var etag = etagValues!.Single();

        var detail = await detailResp.Content.ReadFromJsonAsync<PosudekRoDetailDto>(JsonOptions);
        detail.Should().NotBeNull();
        detail!.Id.Should().Be(id);
        detail.VerzeZaznamu.Should().Be(etag);

        var invalidateReq = new HttpRequestMessage(HttpMethod.Patch, $"/elektronickePosudky/api/v2/posudky/ridicskeOpravneni/{id}/zneplatnit");
        invalidateReq.Headers.TryAddWithoutValidation("If-Match", etag);
        var invalidateResp = await client.SendAsync(invalidateReq);
        invalidateResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var invalidated = await invalidateResp.Content.ReadFromJsonAsync<PosudekRoDetailDto>(JsonOptions);
        invalidated.Should().NotBeNull();
        invalidated!.StavPosudku!.PolozkaKod.Should().Be("ZNEPLATNENY");

        var histResp = await client.GetAsync($"/elektronickePosudky/api/v2/posudky/ridicskeOpravneni/{id}/historie");
        histResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var history = await histResp.Content.ReadFromJsonAsync<List<PosudekRoHistorieDetailDto>>(JsonOptions);
        history.Should().NotBeNull();
        history!.Count.Should().BeGreaterThanOrEqualTo(2);

        var pdfResp = await client.GetAsync($"/elektronickePosudky/api/v2/posudky/ridicskeOpravneni/{id}/pdf");
        pdfResp.StatusCode.Should().Be(HttpStatusCode.OK);
        pdfResp.Content.Headers.ContentType!.MediaType.Should().Be("application/pdf");

        var search = new PosudkyRoSearchRequest { Page = 1, Size = 10, Rid = "RID-123" };
        var searchResp = await client.PostAsJsonAsync("/elektronickePosudky/api/v2/posudky/ridicskeOpravneni/vyhledat", search, JsonOptions);
        searchResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var page = await searchResp.Content.ReadFromJsonAsync<PosudekRoDetailDtoPageResponse>(JsonOptions);
        page.Should().NotBeNull();
        page!.Page.Should().NotBeNull();
        page.Page!.Any(p => p.Id == id).Should().BeTrue();

        var opr = new PosudekRoOpravneniRequestDto { Ico = "12345678", KrzpId = "KRZP-001" };
        var oprResp = await client.PostAsJsonAsync("/elektronickePosudky/api/v2/posudky/ridicskeOpravneni/zalozeni/opravneni", opr, JsonOptions);
        oprResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var oprBody = await oprResp.Content.ReadFromJsonAsync<PosudekRoOpravneniResponseDto>(JsonOptions);
        oprBody!.Opravneni.Should().BeTrue();
    }
}

