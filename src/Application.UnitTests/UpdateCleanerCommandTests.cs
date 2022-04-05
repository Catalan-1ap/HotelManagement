using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.CleanerEntity.UpdateCleaner;
using Application.Exceptions;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;


namespace Application.UnitTests;


public sealed class UpdateCleanerCommandTests : BaseTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly UpdateCleanerCommandHandler _handler;


    public UpdateCleanerCommandTests()
    {
        _dbContext = MakeContext();
        _handler = new(_dbContext);
    }


    [Fact]
    public async Task ShouldUpdateWhenExist()
    {
        // Arrange
        var cleaner = new Cleaner()
        {
            Person = new()
            {
                FirstName = "F",
                SurName = "S",
                Patronymic = "P"
            }
        };
        await Add(cleaner);
        cleaner.Workdays.Clear();
        cleaner.Person!.SurName = "TestSurname";

        var request = new UpdateCleanerCommand(cleaner);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().BeEquivalentTo(request.Cleaner);
        _dbContext.Cleaners.Should().ContainSingle(c => c.Id == response.Id);
    }


    [Fact]
    public async Task ShouldThrowNotFoundIfDoesntExist()
    {
        // Arrange
        var cleaner = new Cleaner()
        {
            Person = new()
            {
                FirstName = "F",
                SurName = "S",
                Patronymic = "P"
            }
        };
        var request = new UpdateCleanerCommand(cleaner);

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
