using System;
using Domain.Entities;
using MediatR;


namespace Application.CQRS.ClientEntity.CheckOutClient;


public sealed record CheckOutClientCommand(string Passport) : IRequest<RoomReport>;