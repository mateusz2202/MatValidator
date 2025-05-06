using System.Runtime.CompilerServices;

namespace MatValidator;

public interface IValidator
{
    Task<ValidResult> ValidateAsync(object instance, CancellationToken cancellationToken = default);
}

public interface IValidatorBaseProperty { }

public interface IValidatorProperty : IValidatorBaseProperty
{
    string? Validate<T>(T value);
}

public interface IValidatorAsyncProperty : IValidatorBaseProperty
{
    Task<string?> ValidateAsync<T>(T value, CancellationToken cancellationToken);
}

public interface IValidatorRule
{
    IAsyncEnumerable<string> ValidateAsync<T>(T model, CancellationToken cancellationToken = default);
}

internal class BaseValidator
{
    protected readonly string _propertyName;
    protected readonly string? _message;
    public BaseValidator(string propertyName, string? message)
        => (_propertyName, _message) = (propertyName, message);

}

internal class ComparisonValidatorBase<TProperty>(string propertyName, TProperty threshold, string? message)
    : BaseValidator(propertyName, message)
{
    protected readonly TProperty _threshold = threshold;
}

