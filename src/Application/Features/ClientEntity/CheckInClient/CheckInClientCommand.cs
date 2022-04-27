using MediatR;


namespace Application.Features.ClientEntity.CheckInClient;


public sealed record CheckInClientCommand(string Passport, string City, string RoomNumber) : IRequest;
