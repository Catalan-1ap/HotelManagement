using Domain.Entities;
using MediatR;


namespace Application.CQRS.CleanerEntity.CreateCleaner;


public sealed record CreateCleanerCommand
    (string FirstName, string SurName, string Patronymic) : IRequest<Cleaner>;
