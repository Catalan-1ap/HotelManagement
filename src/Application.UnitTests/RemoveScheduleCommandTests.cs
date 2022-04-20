using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.CleanerEntity.CreateCleaner;
using Application.CQRS.CleaningScheduleEntity.CreateSchedule;
using Application.CQRS.CleaningScheduleEntity.RemoveSchedule;
using Application.Exceptions;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Xunit;


namespace Application.UnitTests;


public sealed class RemoveScheduleCommandTests : BaseTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly RemoveScheduleCommandHandler _handler;


    public RemoveScheduleCommandTests()
    {
        _dbContext = MakeContext();

        _handler = new(_dbContext);
    }


    [Fact]
    public async Task ShouldRemove_WhenAllAlright()
    {
        // Arrange
        var floor = new Floor { Number = 1 };
        var cleaner = new Cleaner { Person = new() };
        const Weekday workday = Weekday.Friday;

        await AddFloor(floor);
        cleaner = await AddCleaner(cleaner);
        await AddSchedule(floor, cleaner, workday);

        var command = new RemoveScheduleCommand(floor.Number, cleaner.Id, workday);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _dbContext.CleaningSchedule.Should().NotContain(s =>
            s.FloorId == floor.Number &&
            s.CleanerId == cleaner.Id &&
            s.Weekday == workday);
    }


    [Fact]
    public async Task ShouldThrow_WhenScheduleDoesntExists()
    {
        // Arrange
        var floor = new Floor { Number = 1 };
        var cleaner = new Cleaner { Person = new() };
        const Weekday workday = Weekday.Friday;

        await AddFloor(floor);
        cleaner = await AddCleaner(cleaner);

        var command = new RemoveScheduleCommand(floor.Number, cleaner.Id, workday);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(CleaningSchedule));
    }


    private async Task AddFloor(Floor floor)
    {
        var context = MakeContext();

        context.Floors.Add(floor);

        await context.SaveChangesAsync(CancellationToken.None);
    }


    private async Task<Cleaner> AddCleaner(Cleaner cleaner)
    {
        var createCommand = new CreateCleanerCommand(
            cleaner.Person!.FirstName,
            cleaner.Person.SurName,
            cleaner.Person.Patronymic);

        var handler = new CreateCleanerCommandHandler(MakeContext());

        return await handler.Handle(createCommand, CancellationToken.None);
    }


    private async Task AddSchedule(Floor floor, Cleaner cleaner, Weekday workday)
    {
        var command = new CreateScheduleCommand(floor.Number, cleaner.Id, workday);
        var handler = new CreateScheduleCommandHandler(MakeContext());

        await handler.Handle(command, CancellationToken.None);
    }
}
