using Domain.Entities;
using MediatR;


namespace Application.CleanerCQRS.Commands.CreateCleaner;


public sealed record CreateCleanerCommand
    (string FirstName, string SurName, string Patronymic) : IRequest<Cleaner>;
