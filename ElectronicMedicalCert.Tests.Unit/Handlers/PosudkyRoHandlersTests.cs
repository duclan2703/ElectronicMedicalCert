using ElectronicMedicalCert.Application.Common.Exceptions;
using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CheckOpravneni;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CreatePosudekRo;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.InvalidatePosudekRo;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoDetail;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoHistorie;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoPdf;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.SearchPosudkyRo;
using FluentAssertions;
using Xunit;

namespace ElectronicMedicalCert.Tests.Unit.Handlers;

public sealed class PosudkyRoHandlersTests
{
    private static PosudekRoCreateDto ValidCreateDto() => new()
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

    [Fact]
    public async Task CheckOpravneni_returns_true_for_seeded_pair()
    {
        await using var db = await TestDbFactory.CreateSeededAsync();
        var handler = new CheckOpravneniCommandHandler(db);

        var res = await handler.Handle(new CheckOpravneniCommand(new PosudekRoOpravneniRequestDto { Ico = "12345678", KrzpId = "KRZP-001" }), CancellationToken.None);

        res.Opravneni.Should().BeTrue();
    }

    [Fact]
    public async Task Create_then_get_detail_returns_etag_verzeZaznamu()
    {
        await using var db = await TestDbFactory.CreateSeededAsync();

        var createHandler = new CreatePosudekRoCommandHandler(db);
        var created = await createHandler.Handle(new CreatePosudekRoCommand(ValidCreateDto()), CancellationToken.None);

        created.Zpusobilosti.Should().NotBeNullOrEmpty();
        var id = created.Zpusobilosti![0].PosudekId;

        var detailHandler = new GetPosudekRoDetailQueryHandler(db);
        var detail = await detailHandler.Handle(new GetPosudekRoDetailQuery(id), CancellationToken.None);

        detail.Id.Should().Be(id);
        detail.VerzeZaznamu.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Get_detail_unknown_id_throws_NotFound()
    {
        await using var db = await TestDbFactory.CreateSeededAsync();
        var handler = new GetPosudekRoDetailQueryHandler(db);

        var act = async () => await handler.Handle(new GetPosudekRoDetailQuery(Guid.NewGuid()), CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Search_returns_paged_results()
    {
        await using var db = await TestDbFactory.CreateSeededAsync();
        var createHandler = new CreatePosudekRoCommandHandler(db);

        var dto1 = ValidCreateDto();
        dto1.Rid = "RID-A";
        await createHandler.Handle(new CreatePosudekRoCommand(dto1), CancellationToken.None);

        var dto2 = ValidCreateDto();
        dto2.Rid = "RID-B";
        await createHandler.Handle(new CreatePosudekRoCommand(dto2), CancellationToken.None);

        var handler = new SearchPosudkyRoQueryHandler(db);
        var page = await handler.Handle(new SearchPosudkyRoQuery(new PosudkyRoSearchRequest { Page = 1, Size = 10 }), CancellationToken.None);

        page.TotalCount.Should().BeGreaterThanOrEqualTo(2);
        page.Page.Should().NotBeNull();
        page.Page!.Count.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task History_includes_create_operation()
    {
        await using var db = await TestDbFactory.CreateSeededAsync();
        var createHandler = new CreatePosudekRoCommandHandler(db);
        var created = await createHandler.Handle(new CreatePosudekRoCommand(ValidCreateDto()), CancellationToken.None);
        var id = created.Zpusobilosti![0].PosudekId;

        var handler = new GetPosudekRoHistorieQueryHandler(db);
        var history = await handler.Handle(new GetPosudekRoHistorieQuery(id), CancellationToken.None);

        history.Should().NotBeNullOrEmpty();
        history!.Select(h => h.TypOperace!.PolozkaKod).Should().Contain("CREATE");
    }

    [Fact]
    public async Task Pdf_returns_minimal_pdf_bytes()
    {
        await using var db = await TestDbFactory.CreateSeededAsync();
        var createHandler = new CreatePosudekRoCommandHandler(db);
        var created = await createHandler.Handle(new CreatePosudekRoCommand(ValidCreateDto()), CancellationToken.None);
        var id = created.Zpusobilosti![0].PosudekId;

        var handler = new GetPosudekRoPdfQueryHandler(db);
        var pdf = await handler.Handle(new GetPosudekRoPdfQuery(id), CancellationToken.None);

        pdf.Content.Should().NotBeNullOrEmpty();
        System.Text.Encoding.ASCII.GetString(pdf.Content.Take(4).ToArray()).Should().Be("%PDF");
        pdf.FileName.Should().EndWith(".pdf");
    }

    [Fact]
    public async Task Invalidate_wrong_etag_throws_Conflict()
    {
        await using var db = await TestDbFactory.CreateSeededAsync();
        var createHandler = new CreatePosudekRoCommandHandler(db);
        var created = await createHandler.Handle(new CreatePosudekRoCommand(ValidCreateDto()), CancellationToken.None);
        var id = created.Zpusobilosti![0].PosudekId;

        var handler = new InvalidatePosudekRoCommandHandler(db);
        var act = async () => await handler.Handle(new InvalidatePosudekRoCommand(id, "WRONG"), CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task Invalidate_changes_state_and_adds_history()
    {
        await using var db = await TestDbFactory.CreateSeededAsync();
        var createHandler = new CreatePosudekRoCommandHandler(db);
        var created = await createHandler.Handle(new CreatePosudekRoCommand(ValidCreateDto()), CancellationToken.None);
        var id = created.Zpusobilosti![0].PosudekId;

        var detailHandler = new GetPosudekRoDetailQueryHandler(db);
        var before = await detailHandler.Handle(new GetPosudekRoDetailQuery(id), CancellationToken.None);

        // Clear tracked entities to avoid identity map conflicts in InMemory provider.
        db.ChangeTracker.Clear();

        var invalidateHandler = new InvalidatePosudekRoCommandHandler(db);
        var after = await invalidateHandler.Handle(new InvalidatePosudekRoCommand(id, before.VerzeZaznamu!), CancellationToken.None);

        after.StavPosudku!.PolozkaKod.Should().Be("ZNEPLATNENY");

        var historyHandler = new GetPosudekRoHistorieQueryHandler(db);
        var history = await historyHandler.Handle(new GetPosudekRoHistorieQuery(id), CancellationToken.None);
        history.Select(h => h.TypOperace!.PolozkaKod).Should().Contain("INVALIDATE");
    }
}

