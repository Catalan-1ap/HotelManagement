using Application.CleanerCQRS.Commands.RemoveCleaner;
using Application.UnitTests.Common;


namespace Application.UnitTests;


public class RemoveCleanerCommandTests : BaseTestHandler
{
    private readonly RemoveCleanerCommandHandler _handler;


    public RemoveCleanerCommandTests()
    {
        _handler = new(DbContext);
    }
}
