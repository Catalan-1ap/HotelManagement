using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.CleanerEntity.CreateCleaner;
using Application.CQRS.CleaningScheduleEntity.CreateSchedule;
using Application.Exceptions;
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
    public async Task ShouldCreateScheduleWhenAllAlright()
    {
        // Arrange
        var floor = await CreateFloor();
        var cleaner = await CreateCleaner();
        var workday = Weekday.Friday;
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
    public async Task ShouldThrowWhenScheduleAlreadyExists()
    {
        // Arrange
        var floor = await CreateFloor();
        var cleaner = await CreateCleaner();
        var workday = Weekday.Friday;
        var command = new CreateScheduleCommand(floor.Number, cleaner.Id, workday);
        await _handler.Handle(command, CancellationToken.None);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<CleaningScheduleAlreadyExistsException>())
            .Where(e => e.Weekday == workday)
            .Where(e => e.CleanerId == cleaner.Id)
            .Where(e => e.FloorNumber == floor.Number);
    }


    [Fact]
    public async Task ShouldThrowWhenFloorDoesntExists()
    {
        // Arrange
        var floorNumber = 1;
        var cleaner = await CreateCleaner();
        var workday = Weekday.Friday;
        var command = new CreateScheduleCommand(floorNumber, cleaner.Id, workday);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(Floor))
            .Where(e => (int)e.Key == floorNumber);
    }


    [Fact]
    public async Task ShouldThrowWhenCleanerDoesntExists()
    {
        // Arrange
        var floor = await CreateFloor();
        var cleanerId = 1;
        var workday = Weekday.Friday;
        var command = new CreateScheduleCommand(floor.Number, cleanerId, workday);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(Cleaner))
            .Where(e => (int)e.Key == cleanerId);
    }


    private async Task<Floor> CreateFloor()
    {
        var floor = new Floor
        {
            Number = 1
        };

        var context = MakeContext();
        context.Floors.Add(floor);
        await context.SaveChangesAsync(CancellationToken.None);

        return floor;
    }


    private async Task<Cleaner> CreateCleaner()
    {
        var createCommand = new CreateCleanerCommand("FirstName", "SurName", "Patronymic");
        var handler = new CreateCleanerCommandHandler(MakeContext());

        var cleaner = await handler.Handle(createCommand, CancellationToken.None);

        return cleaner;
    }
}
