using System;
using Application.Interfaces;
using Application.UnitTests.Mocks;
using FluentAssertions;
using MediatR;


namespace Application.UnitTests.Common;


public class BaseTestHandler
{
    private readonly string _dbName;
    
    
    protected BaseTestHandler()
    {
        _dbName = Guid.NewGuid().ToString();
        
        AssertionOptions.AssertEquivalencyUsing(options => options.IgnoringCyclicReferences());
    }


    protected IApplicationDbContext MakeContext() => MockedContext.Make(_dbName);
}
