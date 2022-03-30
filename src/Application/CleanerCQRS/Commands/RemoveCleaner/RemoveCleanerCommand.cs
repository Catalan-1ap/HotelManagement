using MediatR;


namespace Application.CleanerCQRS.Commands.RemoveCleaner;


public sealed record RemoveCleanerCommand(int Id) : IRequest;
