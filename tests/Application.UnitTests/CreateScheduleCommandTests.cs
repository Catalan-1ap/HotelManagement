using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Features.CleanerEntity.CreateCleaner;
using Application.Features.CleaningScheduleEntity.CreateSchedule;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Xunit;


namespace Application.UnitTests;


public sealed class CreateScheduleCommandTests : BaseTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly CreateScheduleCommandHandler _handler;


    public CreateScheduleCommandTests()
    {
        _dbContext = MakeContext();

        _handler = new(_dbContext);
    }


    [Fact]
    public async Task ShouldCreate_WhenAllAlright()
    {
        // Arrange
        var floor = new Floor { Number = 1 };
        var cleaner = new Cleaner();
        const Weekday workday = Weekday.Saturday;

        await RegisterFloor(floor);
        cleaner = await RegisterCleaner(cleaner);

        var command = new CreateScheduleCommand(floor.Number, cleaner.Id, workday);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _dbContext.CleaningSchedule.Should().Contain(s =>
            s.FloorId == floor.Number &&
            s.CleanerId == cleaner.Id &&
            s.Weekday == workday);
    }


    [Fact]
    public async Task ShouldThrow_WhenScheduleForFloorAlreadyExists()
    {
        // Arrange
        var floor = new Floor { Number = 1 };
        var cleaner = new Cleaner();
        const Weekday workday = Weekday.Saturday;

        await RegisterFloor(floor);
        cleaner = await RegisterCleaner(cleaner);

        var command = new CreateScheduleCommand(floor.Number, cleaner.Id, workday);
        await _handler.Handle(command, CancellationToken.None);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<CleaningScheduleForFloorAlreadyExistsException>())
            .Where(e => e.Weekday == workday)
            .Where(e => e.FloorNumber == floor.Number);
    }


    [Fact]
    public async Task ShouldThrow_WhenScheduleForCleanerAlreadyExists()
    {
        // Arrange
        var mainFloor = new Floor { Number = 1 };
        var secondaryFloor = new Floor { Number = 2 };
        var cleaner = new Cleaner();
        const Weekday workday = Weekday.Saturday;

        await RegisterFloor(mainFloor);
        await RegisterFloor(secondaryFloor);
        cleaner = await RegisterCleaner(cleaner);

        var command = new CreateScheduleCommand(mainFloor.Number, cleaner.Id, workday);
        await _handler.Handle(command, CancellationToken.None);
        command = new(secondaryFloor.Number, cleaner.Id, workday);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<CleaningScheduleForCleanerAlreadyExistsException>())
            .Where(e => e.Weekday == workday)
            .Where(e => e.CleanerId == cleaner.Id);
    }


    [Fact]
    public async Task ShouldThrow_WhenFloorDoesntExists()
    {
        // Arrange
        const int floorNumber = 1;
        var cleaner = new Cleaner();
        const Weekday workday = Weekday.Saturday;

        cleaner = await RegisterCleaner(cleaner);

        var command = new CreateScheduleCommand(floorNumber, cleaner.Id, workday);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(Floor))
            .Where(e => (int)e.Key == floorNumber);
    }


    [Fact]
    public async Task ShouldThrow_WhenCleanerDoesntExists()
    {
        // Arrange
        const int cleanerId = 1;
        var floor = new Floor { Number = 1 };
        const Weekday workday = Weekday.Saturday;

        await RegisterFloor(floor);

        var command = new CreateScheduleCommand(floor.Number, cleanerId, workday);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(Cleaner))
            .Where(e => (int)e.Key == cleanerId);
    }


    private async Task RegisterFloor(Floor floor)
    {
        var context = MakeContext();

        context.Floors.Add(floor);
        await context.SaveChangesAsync(CancellationToken.None);
    }


    private async Task<Cleaner> RegisterCleaner(Cleaner cleaner)
    {
        var createCommand = new CreateCleanerCommand(
            cleaner.FirstName,
            cleaner.SurName,
            cleaner.Patronymic);

        var handler = new CreateCleanerCommandHandler(MakeContext());

        return await handler.Handle(createCommand, CancellationToken.None);
    }
}
