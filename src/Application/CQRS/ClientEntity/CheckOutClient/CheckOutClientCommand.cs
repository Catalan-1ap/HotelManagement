using Domain.Entities;
using MediatR;


namespace Application.CQRS.ClientEntity.CheckOutClient;


public sealed record CheckOutClientCommand(string PayerPassport) : IRequest<RoomReport>;
