using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.CQRS.ClientEntity.CheckOutClient;


public sealed class CheckOutClientCommandHandler : IRequestHandler<CheckOutClientCommand, RoomReport>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTimeService _dateTimeService;


    public CheckOutClientCommandHandler(IApplicationDbContext dbContext, IDateTimeService dateTimeService)
    {
        _dbContext = dbContext;
        _dateTimeService = dateTimeService;
    }


    public async Task<RoomReport> Handle(CheckOutClientCommand request, CancellationToken token)
    {
        var client = await TryGetClient(request.Passport, token);
        
        var report = CreateReport(client);
        client.IsCheckout = true;
        client.Room = null;
        
        _dbContext.RoomReports.Add(report);
        await _dbContext.SaveChangesAsync(token);

        return report;
    }


    private async Task<Client> TryGetClient(string passport, CancellationToken token)
    {
        var client = await _dbContext.Clients
            .Include(c => c.Room)
            .ThenInclude(r => r!.RoomType)
            .Where(Client.CanCheckOut)
            .SingleOrDefaultAsync(c => c.Passport == passport, token);
        
        if (client is null)
            throw new NotFoundException(nameof(Client), passport);

        return client;
    }


    private RoomReport CreateReport(Client client)
    {
        var days = (_dateTimeService.UtcNow - client.Arrival).Days;

        var report = new RoomReport
        {
            Client = client,
            Room = client.Room,
            DaysNumber = days,
            TotalPrice = client.Room!.RoomType!.PricePerDay * days
        };

        return report;
    }
}
