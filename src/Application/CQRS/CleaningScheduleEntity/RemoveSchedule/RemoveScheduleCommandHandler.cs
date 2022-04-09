using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.CQRS.CleaningScheduleEntity.RemoveSchedule;


public sealed class RemoveScheduleCommandHandler : IRequestHandler<RemoveScheduleCommand>
{
    private readonly IApplicationDbContext _dbContext;


    public RemoveScheduleCommandHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;


    public async Task<Unit> Handle(RemoveScheduleCommand request, CancellationToken token)
    {
        var schedule = await TryGetSchedule(request, token);

        _dbContext.CleaningSchedule.Remove(schedule);
        await _dbContext.SaveChangesAsync(token);

        return Unit.Value;
    }


    private async Task<CleaningSchedule> TryGetSchedule(RemoveScheduleCommand request, CancellationToken token)
    {
        var schedule = await _dbContext.CleaningSchedule
            .SingleOrDefaultAsync(s =>
                    s.FloorId == request.FloorNumber &&
                    s.CleanerId == request.CleanerId &&
                    s.Weekday == request.Workday,
                token);

        if (schedule is null)
            throw new NotFoundException(nameof(CleaningSchedule),
                new
                {
                    request.FloorNumber,
                    request.CleanerId,
                    request.Workday
                });

        return schedule;
    }
}
