using AutoMapper;
using ElectronicMedicalCert.Application.Features.Codebooks.Queries.GetCodebookItems;
using ElectronicMedicalCert.Application.Features.Codebooks.Queries.GetCodebooks;
using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Domain.Entities;
using FluentAssertions;
using System.Linq.Expressions;
using Xunit;

namespace ElectronicMedicalCert.Tests.Unit.Handlers;

public sealed class CodebooksHandlersTests
{
    private static IMapper CreateMapper() => new FakeMapper();

    [Fact]
    public async Task GetCodebooks_returns_seeded_codebooks()
    {
        await using var db = await TestDbFactory.CreateSeededAsync();
        var handler = new GetCodebooksQueryHandler(db, CreateMapper());

        var result = await handler.Handle(new GetCodebooksQuery(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Select(x => x.Kod).Should().Contain("STAV-POSUDKU");
    }

    [Fact]
    public async Task GetCodebookItems_filters_by_codebook_kod()
    {
        await using var db = await TestDbFactory.CreateSeededAsync();
        var handler = new GetCodebookItemsQueryHandler(db, CreateMapper());

        var items = await handler.Handle(new GetCodebookItemsQuery("STAV-POSUDKU"), CancellationToken.None);

        items.Should().NotBeNull();
        items.Select(i => i.Kod).Should().Contain(new[] { "stav_posudku_platny", "stav_posudku_zneplatneny" });
    }

    private sealed class FakeMapper : IMapper
    {
        public TDestination Map<TDestination>(object source)
        {
            if (typeof(TDestination) == typeof(CiselnikDto) && source is Codebook cb)
            {
                var dto = new CiselnikDto
                {
                    Id = cb.Id,
                    Kod = cb.Kod,
                    Verze = cb.Verze,
                    PlatnostOd = cb.PlatnostOd,
                    PlatnostDo = cb.PlatnostDo,
                    Termx = cb.Termx,
                    TermxId = cb.TermxId,
                    TermxUrl = cb.TermxUrl,
                    Preklady = ToDict(cb.Preklady),
                };
                return (TDestination)(object)dto;
            }

            if (typeof(TDestination) == typeof(CiselnikPolozkaDto) && source is CodebookItem it)
            {
                var dto = new CiselnikPolozkaDto
                {
                    Id = it.Id,
                    Kod = it.Kod,
                    Verze = it.Verze,
                    RodicId = it.RodicId,
                    Preklady = ToDict(it.Preklady),
                };
                return (TDestination)(object)dto;
            }

            throw new NotSupportedException($"Mapping {source.GetType().Name} -> {typeof(TDestination).Name} not supported.");
        }

        private static Dictionary<string, TranslationDto>? ToDict(List<Translation> translations)
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

        // Unused members for these unit tests
        public IConfigurationProvider ConfigurationProvider => throw new NotImplementedException();
        public Func<Type, object> ServiceCtor => throw new NotImplementedException();
        public object Map(object source, Type sourceType, Type destinationType) => throw new NotImplementedException();
        public object Map(object source, object destination, Type sourceType, Type destinationType) => throw new NotImplementedException();
        public TDestination Map<TSource, TDestination>(TSource source) => Map<TDestination>(source!);
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination) => throw new NotImplementedException();
        public object Map(object source, object destination) => throw new NotImplementedException();
        public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions<object, TDestination>> opts) => throw new NotImplementedException();
        public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts) => throw new NotImplementedException();
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts) => throw new NotImplementedException();
        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts) => throw new NotImplementedException();
        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts) => throw new NotImplementedException();
        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts) => throw new NotImplementedException();
        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts) => throw new NotImplementedException();

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object? parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand) =>
            throw new NotImplementedException();

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object> parameters, params string[] membersToExpand) =>
            throw new NotImplementedException();

        public IQueryable ProjectTo(IQueryable source, Type destinationType, IDictionary<string, object> parameters, params string[] membersToExpand) =>
            throw new NotImplementedException();
    }
}

