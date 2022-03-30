using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.CleanerCQRS.Commands.RemoveCleaner;


public sealed class RemoveCleanerCommandHandler : IRequestHandler<RemoveCleanerCommand>
{
    private readonly IApplicationDbContext _dbContext;


    public RemoveCleanerCommandHandler(IApplicationDbContext dbContext) =>
        _dbContext = dbContext;


    public async Task<Unit> Handle(RemoveCleanerCommand request, CancellationToken token)
    {
        var cleanerToRemove = await _dbContext.Cleaners
            .SingleOrDefaultAsync(c => c.Id == request.Id, token);

        if (cleanerToRemove is null)
            throw new NotFoundException(nameof(Cleaner), request.Id);

        _dbContext.Cleaners.Remove(cleanerToRemove);
        await _dbContext.SaveChangesAsync(token);

        return Unit.Value;
    }
}
