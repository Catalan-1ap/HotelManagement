using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.CleanerCQRS.Commands.UpdateCleanerCommand;


public sealed class UpdateCleanerCommandHandler : IRequestHandler<UpdateCleanerCommand, Cleaner>
{
    private readonly IApplicationDbContext _dbContext;


    public UpdateCleanerCommandHandler(IApplicationDbContext dbContext) =>
        _dbContext = dbContext;


    public async Task<Cleaner> Handle(UpdateCleanerCommand request, CancellationToken token)
    {
        var isCleanerExists = await _dbContext.Cleaners
            .AnyAsync(c => c.Id == request.Cleaner.Id, token);

        if (isCleanerExists == false)
            throw new NotFoundException(nameof(Cleaner), request.Cleaner.Id);

        _dbContext.Cleaners.Update(request.Cleaner);
        await _dbContext.SaveChangesAsync(token);

        return request.Cleaner;
    }
}
