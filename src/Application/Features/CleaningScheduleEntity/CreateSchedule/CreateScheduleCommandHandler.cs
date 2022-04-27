using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.CleaningScheduleEntity.CreateSchedule;


public sealed class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand>
{
    private readonly IApplicationDbContext _dbContext;


    public CreateScheduleCommandHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;


    public async Task<Unit> Handle(CreateScheduleCommand request, CancellationToken token)
    {
        var (floorNumber, cleanerId, workday) = request;

        await CheckScheduleRepeatingViolation(request, token);

        var floor = await TryGetFloor(floorNumber, token);
        var cleaner = await TryGetCleaner(cleanerId, token);

        var schedule = new CleaningSchedule
        {
            Floor = floor,
            Cleaner = cleaner,
            Weekday = workday
        };
        _dbContext.CleaningSchedule.Add(schedule);
        await _dbContext.SaveChangesAsync(token);

        return Unit.Value;
    }


    private async Task CheckScheduleRepeatingViolation(CreateScheduleCommand command, CancellationToken token)
    {
        var (floorNumber, cleanerId, workday) = command;

        var isScheduleExists = new
        {
            IsFloorExists = await _dbContext.CleaningSchedule
                .AnyAsync(s => s.FloorId == floorNumber
                               &&
                               s.Weekday == workday,
                    token),
            IsCleanerExists = await _dbContext.CleaningSchedule
                .AnyAsync(s => s.CleanerId == cleanerId
                               &&
                               s.Weekday == workday,
                    token)
        };

        if (isScheduleExists.IsFloorExists)
            throw new CleaningScheduleForFloorAlreadyExistsException(floorNumber, workday);

        if (isScheduleExists.IsCleanerExists)
            throw new CleaningScheduleForCleanerAlreadyExistsException(cleanerId, workday);
    }


    private async Task<Floor> TryGetFloor(int floorNumber, CancellationToken token)
    {
        var floor = await _dbContext.Floors
            .SingleOrDefaultAsync(f => f.Number == floorNumber, token);

        if (floor is null)
            throw new NotFoundException(nameof(Floor), floorNumber);

        return floor;
    }


    private async Task<Cleaner> TryGetCleaner(int cleanerId, CancellationToken token)
    {
        var cleaner = await _dbContext.Cleaners
            .SingleOrDefaultAsync(c => c.Id == cleanerId, token);

        if (cleaner is null)
            throw new NotFoundException(nameof(Cleaner), cleanerId);

        return cleaner;
    }
}
