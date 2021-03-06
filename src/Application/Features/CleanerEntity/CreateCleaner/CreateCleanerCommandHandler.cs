using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;


namespace Application.Features.CleanerEntity.CreateCleaner;


public sealed class CreateCleanerCommandHandler : IRequestHandler<CreateCleanerCommand, Cleaner>
{
    private readonly IApplicationDbContext _dbContext;


    public CreateCleanerCommandHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;


    public async Task<Cleaner> Handle(CreateCleanerCommand request, CancellationToken token)
    {
        var newCleaner = new Cleaner
        {
            FirstName = request.FirstName,
            SurName = request.SurName,
            Patronymic = request.Patronymic
        };

        _dbContext.Cleaners.Add(newCleaner);
        await _dbContext.SaveChangesAsync(token);

        return newCleaner;
    }
}
