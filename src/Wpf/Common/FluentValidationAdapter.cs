using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Stylet;


namespace Wpf.Common;


#nullable disable
public class FluentModelValidator<T> : IModelValidator<T>
{
    private readonly IValidator<T> _validator;
    private T _subject;

    public FluentModelValidator(IValidator<T> validator) => _validator = validator;


    public void Initialize(object subject) => _subject = (T)subject;


    public async Task<IEnumerable<string>> ValidatePropertyAsync(string propertyName) =>
        // If someone's calling us synchronously, and ValidationAsync does not complete synchronously,
        // we'll deadlock unless we continue on another thread.
        (await _validator.ValidateAsync(_subject, strategy => strategy.IncludeProperties(propertyName)).ConfigureAwait(false))
        .Errors.Select(x => x.ErrorMessage);


    public async Task<Dictionary<string, IEnumerable<string>>> ValidateAllPropertiesAsync() =>
        // If someone's calling us synchronously, and ValidationAsync does not complete synchronously,
        // we'll deadlock unless we continue on another thread.
        (await _validator.ValidateAsync(_subject).ConfigureAwait(false))
        .Errors.GroupBy(x => x.PropertyName)
        .ToDictionary(x => x.Key, x => x.Select(failure => failure.ErrorMessage));
}
#nullable restore
