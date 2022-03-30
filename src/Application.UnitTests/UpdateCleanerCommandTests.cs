using System.Threading;
using System.Threading.Tasks;
using Application.CleanerCQRS.Commands.RemoveCleaner;
using Application.CleanerCQRS.Commands.UpdateCleanerCommand;
using Application.Exceptions;
using Application.Interfaces;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;


namespace Application.UnitTests;


public sealed class UpdateCleanerCommandTests : BaseCleanerTestHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly UpdateCleanerCommandHandler _handler;


    public UpdateCleanerCommandTests() : base(nameof(UpdateCleanerCommandTests))
    {
        _dbContext = GetContext();
        _handler = new(_dbContext);
    }


    [Fact]
    public async Task ShouldUpdateWhenExist()
    {
        // Arrange
        var cleanerToUpdate = await Add();
        cleanerToUpdate.Workdays.Clear();
        cleanerToUpdate.Person!.SurName = "TestSurname";
        
        var request = new UpdateCleanerCommand(cleanerToUpdate);

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
        var request = new UpdateCleanerCommand(TestObject);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task ShouldCallUpdateAndSaveChanges()
    {
        // Arrange
        var cleanerToUpdate = await Add();
        cleanerToUpdate.Workdays.Clear();
        cleanerToUpdate.Person!.SurName = "TestSurname";
        
        var request = new UpdateCleanerCommand(cleanerToUpdate);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _dbContext.Cleaners.Received(Quantity.Exactly(1)).Update(Arg.Any<Cleaner>());
        await _dbContext.Received(Quantity.Exactly(1)).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    
    private async Task<Cleaner> Add()
    {
        var context = GetContext();

        context.Cleaners.Add(TestObject);
        await context.SaveChangesAsync(CancellationToken.None);

        return TestObject;
    }
}
