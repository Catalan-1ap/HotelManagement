using Domain.Entities;
using MediatR;


namespace Application.CQRS.ClientEntity.CreateClient;


public sealed record CreateClientCommand
    (string Passport, string City, Person Person) : IRequest<Client>;
