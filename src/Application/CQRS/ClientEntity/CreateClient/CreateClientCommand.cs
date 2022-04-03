using Domain.Entities;
using MediatR;


namespace Application.CQRS.ClientEntity.CreateClient;


public sealed record CreateClientCommand(string Passport, Person Person) : IRequest<Client>;
