using System.Collections.Generic;
using Domain.Entities;


namespace Application.Features.ClientEntity.CheckOutClient;


public sealed record CheckOutClientCommandResponse(
    Client Payer,
    string RoomNumber,
    int DaysNumber,
    int TotalPrice,
    int PricePerDay,
    IReadOnlyCollection<Client> Party
);
