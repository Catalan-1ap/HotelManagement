using MediatR;


namespace Application.CQRS.CleanerEntity.RemoveCleaner;


public sealed record RemoveCleanerCommand(int Id) : IRequest;
