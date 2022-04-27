using MediatR;


namespace Application.Features.CleanerEntity.RemoveCleaner;


public sealed record RemoveCleanerCommand(int Id) : IRequest;
