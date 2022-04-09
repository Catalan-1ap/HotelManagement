using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.CleanerEntity.RemoveCleaner;
using Application.Exceptions;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using Xunit;


namespace Application.UnitTests;


public class RemoveCleanerCommandTests : BaseTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly RemoveCleanerCommandHandler _handler;


    public RemoveCleanerCommandTests()
    {
        _dbContext = MakeContext();
        _handler = new(_dbContext);
    }


    [Fact]
    public async Task ShouldRemoveWhenExist()
    {
        // Arrange
        var cleaner = new Cleaner
        {
            Person = new()
            {
                FirstName = "F",
                SurName = "S",
                Patronymic = "P"
            }
        };
        await Add(cleaner);
        var request = new RemoveCleanerCommand(cleaner.Id);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Cleaners.Should().NotContain(cleaner);
    }


    [Fact]
    public async Task ShouldThrowNotFoundIfDoesntExist()
    {
        // Arrange
        var request = new RemoveCleanerCommand(1);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    private async Task Add(Cleaner cleaner)
    {
        var context = MakeContext();

        context.Cleaners.Add(cleaner);
        await context.SaveChangesAsync(CancellationToken.None);
    }
}
