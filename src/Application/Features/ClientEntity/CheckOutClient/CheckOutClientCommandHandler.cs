using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.ClientEntity.CheckOutClient;


public sealed class CheckOutClientCommandHandler : IRequestHandler<CheckOutClientCommand, CheckOutClientCommandResponse>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IApplicationDbContext _dbContext;


    public CheckOutClientCommandHandler(IApplicationDbContext dbContext, IDateTimeService dateTimeService)
    {
        _dbContext = dbContext;
        _dateTimeService = dateTimeService;
    }


    public async Task<CheckOutClientCommandResponse> Handle(CheckOutClientCommand request, CancellationToken token)
    {
        var payer = await TryGetClient(request.PayerPassport, token);
        var party = await GetRestClientsExcludePayer(payer.Passport!, payer.Room!.Number!, token);

        var report = CreateReport(payer);
        CheckOutClients(payer, party);

        _dbContext.RoomReports.Add(report);
        await _dbContext.SaveChangesAsync(token);

        return new(
            payer,
            report.RoomId,
            report.DaysNumber,
            report.TotalPrice,
            report.Room!.RoomType!.PricePerDay,
            party
        );
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


    private async Task<IReadOnlyCollection<Client>> GetRestClientsExcludePayer(
        string payerPassport,
        string roomNumber,
        CancellationToken token)
    {
        var party = await _dbContext.Rooms
            .Include(r => r.Clients)
            .Where(r => r.Number == roomNumber)
            .SelectMany(r => r.Clients)
            .Where(c => c.Passport != payerPassport)
            .ToListAsync(token);

        return party;
    }


    private RoomReport CreateReport(Client payer)
    {
        var now = _dateTimeService.Now;
        var elapsed = (TimeSpan)(now - payer.Arrival!);
        var days = elapsed.Days;

        var report = new RoomReport
        {
            Client = payer,
            Room = payer.Room,
            DaysNumber = days,
            TotalPrice = payer.Room!.RoomType!.PricePerDay * days,
            Arrival = payer.Arrival!.Value,
            Depart = now
        };

        return report;
    }


    private static void CheckOutClients(Client payer, IReadOnlyCollection<Client> party)
    {
        payer.IsCheckout = true;
        payer.Room = null;
        payer.City = string.Empty;
        payer.Arrival = null;

        foreach (var client in party)
        {
            client.IsCheckout = true;
            client.Room = null;
            client.City = string.Empty;
            client.Arrival = null;
        }
    }
}
