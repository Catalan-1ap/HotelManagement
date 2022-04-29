using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.DateReport;


public sealed class DateReportQueryHandler : IRequestHandler<DateReportQuery, DateReportResponse>
{
    private readonly IReadOnlyApplicationDbContext _dbContext;
    private Expression<Func<RoomReport, bool>> _dateInRange;


    public DateReportQueryHandler(IReadOnlyApplicationDbContext dbContext) => _dbContext = dbContext;


    public async Task<DateReportResponse> Handle(DateReportQuery request, CancellationToken token)
    {
        _dateInRange = r =>
            r.Arrival >= request.From
            && r.Arrival <= request.To
            && r.Depart >= request.From
            && r.Depart <= request.To;

        var clientsCount = await _dbContext.RoomReports
            .Where(_dateInRange)
            .Select(r => r.ClientId)
            .Distinct()
            .CountAsync(token);

        var daysCount = (request.To - request.From).Days;

        var roomsDetails = await _dbContext.RoomReports
            .Where(_dateInRange)
            .GroupBy(r => r.RoomId)
            .Select(reports => new
            {
                RoomNumber = reports.Key,
                DaysRoomBusy = reports.Sum(r => r.DaysNumber),
                TotalIncome = reports.Sum(r => r.TotalPrice)
            })
            .Select(arg => new
                DateReportRoomDetails(arg.RoomNumber,
                    arg.TotalIncome,
                    arg.DaysRoomBusy,
                    Math.Abs(arg.DaysRoomBusy - daysCount)
                )
            )
            .ToListAsync(token);

        return new(clientsCount, daysCount, roomsDetails);
    }
}
