using Domain.Entities;
using MediatR;


namespace Application.Features.ClientEntity.CreateClient;


public sealed record CreateClientCommand(string Passport, string FirstName, string SurName, string Patronymic) : IRequest<Client>;
