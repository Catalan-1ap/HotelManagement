using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.CQRS.ClientEntity.CreateClient;


public sealed class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Client>
{
    private readonly IApplicationDbContext _dbContext;

    
    public CreateClientCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Client> Handle(CreateClientCommand request, CancellationToken token)
    {
        var client = request.Adapt<Client>();
        
        _dbContext.Clients.Add(client);
        await _dbContext.SaveChangesAsync(token);

        return client;
    }
}
