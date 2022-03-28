using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;


namespace Application.Exceptions;


public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }


    public ValidationException(IEnumerable<ValidationFailure> failures) =>
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
}
