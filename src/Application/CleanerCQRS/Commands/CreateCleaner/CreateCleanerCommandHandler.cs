using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Mapster;
using MapsterMapper;
using MediatR;


namespace Application.CleanerCQRS.Commands.CreateCleaner;


public sealed class CreateCleanerCommandHandler : IRequestHandler<CreateCleanerCommand, Cleaner>
{
    private readonly IApplicationDbContext _dbContext;


    public CreateCleanerCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Cleaner> Handle(CreateCleanerCommand request, CancellationToken token)
    {
        var newCleaner = new Cleaner
        {
            Person = request.Adapt<Person>()
        };

        _dbContext.Cleaners.Add(newCleaner);
        await _dbContext.SaveChangesAsync(token);

        return newCleaner;
    }
}
