using System.Threading;
using System.Threading.Tasks;
using Application.Features.DateReport;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Xunit;


namespace Application.UnitTests;


public class DateReportQueryTests : BaseTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly DateReportQueryHandler _handler;


    public DateReportQueryTests()
    {
        _dbContext = MakeContext();

        _handler = new(new ReadOnlyApplicationDbContext(_dbContext));
    }


    [Fact]
    public async Task ShouldGiveRightOutput()
    {
        // Arrange
        var report = await Arrange();
        var request = new DateReportQuery(report.Arrival, report.Depart);
        var daysElapsed = (report.Depart - report.Arrival).Days;

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.ClientsCount.Should().Be(1);
        response.DaysCount.Should().Be(report.DaysNumber);

        var roomDetails = response.RoomsDetails.Should().ContainSingle().Which;
        roomDetails.RoomNumber.Should().BeEquivalentTo(report.RoomId);
        roomDetails.TotalIncome.Should().Be(report.TotalPrice);
        roomDetails.DaysRoomBusy.Should().Be(report.DaysNumber);
        roomDetails.DaysRoomFree.Should().Be(report.DaysNumber - daysElapsed);
    }


    private async Task<RoomReport> Arrange()
    {
        var context = MakeContext();

        var first = new RoomReport
        {
            DaysNumber = 7,
            TotalPrice = 7000,
            Arrival = new(2022, 4, 22),
            Depart = new(2022, 4, 29),
            RoomId = "Room"
        };
        context.RoomReports.Add(first);

        await context.SaveChangesAsync(CancellationToken.None);

        return first;
    }
}
