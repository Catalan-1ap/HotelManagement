using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.ClientEntity.CreateClient;


public sealed class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Client>
{
    private readonly IApplicationDbContext _dbContext;


    public CreateClientCommandHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;


    public async Task<Client> Handle(CreateClientCommand request, CancellationToken token)
    {
        await ThrowIfPassportAlreadyExists(request.Passport, token);

        var client = new Client
        {
            Passport = request.Passport,
            FirstName = request.FirstName,
            SurName = request.SurName,
            Patronymic = request.Patronymic
        };

        _dbContext.Clients.Add(client);
        await _dbContext.SaveChangesAsync(token);

        return client;
    }


    private async Task ThrowIfPassportAlreadyExists(string passport, CancellationToken token)
    {
        var isPassportNotExists = await IsSamePassportNotAlreadyExists(passport, token);

        if (isPassportNotExists == false)
            throw new ClientWithPassportAlreadyExistsException(passport);
    }


    private Task<bool> IsSamePassportNotAlreadyExists(string passport, CancellationToken token) => _dbContext.Clients.AllAsync(c => c.Passport != passport, token);
}
