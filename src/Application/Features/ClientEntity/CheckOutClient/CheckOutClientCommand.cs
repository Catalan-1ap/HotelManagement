using MediatR;


namespace Application.Features.ClientEntity.CheckOutClient;


public sealed record CheckOutClientCommand(string PayerPassport) : IRequest<CheckOutClientCommandResponse>;
