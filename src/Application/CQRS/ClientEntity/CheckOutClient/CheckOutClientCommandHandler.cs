﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        var payer = await TryGetClient(request.PayerPassport, token);
        var party = await GetRestClientsExcludePayer(payer.Passport, payer.Room!.Number, token);

        var report = CreateReport(payer);
        CheckOutClients(payer, party);

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


    private async Task<IEnumerable<Client>> GetRestClientsExcludePayer(
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

        return party.Count == 0
            ? Enumerable.Empty<Client>()
            : party;
    }


    private RoomReport CreateReport(Client payer)
    {
        var days = (_dateTimeService.UtcNow - payer.Arrival).Days;

        var report = new RoomReport
        {
            Client = payer,
            Room = payer.Room,
            DaysNumber = days,
            TotalPrice = payer.Room!.RoomType!.PricePerDay * days
        };

        return report;
    }


    private static void CheckOutClients(Client payer, IEnumerable<Client> party)
    {
        payer.IsCheckout = true;
        payer.Room = null;
        
        foreach (var client in party)
        {
            client.IsCheckout = true;
            client.Room = null;
        }
    }
}
