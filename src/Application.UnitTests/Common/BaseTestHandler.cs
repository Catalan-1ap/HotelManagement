using Application.Interfaces;
using Application.UnitTests.Mocks;
using FluentAssertions;


namespace Application.UnitTests.Common;


public class BaseTestHandler
{
    protected readonly IApplicationDbContext DbContext;


    public BaseTestHandler()
    {
        DbContext = MockedContext.Make;

        AssertionOptions.AssertEquivalencyUsing(options => options.IgnoringCyclicReferences());
    }
}
