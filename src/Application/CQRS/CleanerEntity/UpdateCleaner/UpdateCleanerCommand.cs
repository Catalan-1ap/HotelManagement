using Domain.Entities;
using MediatR;


namespace Application.CQRS.CleanerEntity.UpdateCleaner;


public sealed record UpdateCleanerCommand(Cleaner Cleaner) : IRequest<Cleaner>;
