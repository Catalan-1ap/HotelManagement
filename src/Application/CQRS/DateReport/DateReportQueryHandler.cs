using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.CQRS.DateReport;


public sealed class DateReportQueryHandler : IRequestHandler<DateReportQuery, DateReportResponse>
{
    private readonly IReadOnlyApplicationDbContext _dbContext;


    public DateReportQueryHandler(IReadOnlyApplicationDbContext dbContext) => _dbContext = dbContext;

    
    public async Task<DateReportResponse> Handle(DateReportQuery request, CancellationToken token)
    {
        var clientsCount = await _dbContext.Clients
            .Where(c => c.Arrival >= request.From && c.Arrival <= request.To)
            .CountAsync(token);

        var daysCount = (request.To - request.From).Days;

        var roomsDetails = await _dbContext.RoomReports
            .Include(r => r.Client)
            .Where(c => c.Client.Arrival >= request.From && c.Client.Arrival <= request.To)
            .GroupBy(r => r.RoomId)
            .Select(reports => new
            {
                DaysRoomBusy = reports.Sum(r => r.DaysNumber),
                TotalIncome = reports.Sum(r => r.TotalPrice)
            })
            .Select(arg => new
                DateReportRoomDetails(arg.TotalIncome, arg.DaysRoomBusy, daysCount - arg.DaysRoomBusy))
            .ToListAsync(token);

        return new(clientsCount, roomsDetails);
    }
}
