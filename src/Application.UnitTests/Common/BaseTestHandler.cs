using Application.Interfaces;
using Application.UnitTests.Mocks;
using FluentAssertions;


namespace Application.UnitTests.Common;


public class BaseTestHandler
{
    private readonly string _dbName;


    protected BaseTestHandler(string dbName)
    {
        _dbName = dbName;

        AssertionOptions.AssertEquivalencyUsing(options => options.IgnoringCyclicReferences());
    }


    protected IApplicationDbContext GetContext() => MockedContext.Make(_dbName);
}
