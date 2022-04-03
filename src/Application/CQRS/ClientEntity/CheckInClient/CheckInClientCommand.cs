using MediatR;


namespace Application.CQRS.ClientEntity.CheckInClient;


public sealed record CheckInClientCommand(string Passport, string City, string RoomNumber) : IRequest;
