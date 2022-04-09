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


    public async Task ShouldRemoveWhenAllAlright()
    {
        // Arrange
        var floor = await CreateFloor();
        var cleaner = await CreateCleaner();
        var workday = Weekday.Friday;
        await AddSchedule(floor.Number, cleaner.Id, workday);

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
    public async Task ShouldThrowWhenScheduleDoesntExists()
    {
        // Arrange
        var floor = await CreateFloor();
        var cleaner = await CreateCleaner();
        var workday = Weekday.Friday;

        var command = new RemoveScheduleCommand(floor.Number, cleaner.Id, workday);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.EntityName == nameof(CleaningSchedule));
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


    private async Task AddSchedule(int floorNumber, int cleanerId, Weekday workday)
    {
        var command = new CreateScheduleCommand(floorNumber, cleanerId, workday);
        var handler = new CreateScheduleCommandHandler(MakeContext());

        await handler.Handle(command, CancellationToken.None);
    }
}
