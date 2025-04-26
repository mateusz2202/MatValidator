namespace MatValidator;
public sealed partial class RuleBuilder<TModel, TProperty> : IValidatorRule
{
    public RuleBuilder<TModel, TProperty> Must(Func<TProperty, bool> func, string message = null)
        => AddValidator(new MustValidator<TModel, TProperty>(_propertyName, func, message));

    public RuleBuilder<TModel, TProperty> Custom(Func<bool> func, string message = null)
        => AddValidator(new CustomValidator<TModel, TProperty>(_propertyName, func, message));


    public RuleBuilder<TModel, TProperty> SetValidator(ValidatorBuilder<TProperty> validator)
        => AddValidator(new SetValidatorValidator<TModel, TProperty>(_propertyName, _parent, validator, null));

    public RuleBuilder<TModel, TProperty> Equal(TProperty expected, string message = null)
        => AddValidator(new EqualValidator<TModel, TProperty>(_propertyName, expected, message));


    public RuleBuilder<TModel, TProperty> NotEqual(TProperty unexpected, string message = null)
        => AddValidator(new NotEqualValidator<TModel, TProperty>(_propertyName, unexpected, message));


    public RuleBuilder<TModel, TProperty> IsEmpty(string message = null)
        => AddValidator(new IsEmptyValidator<TModel, TProperty>(_propertyName, message));


    public RuleBuilder<TModel, TProperty> NotEmpty(string message = null)
        => AddValidator(new NotEmptyValidator<TModel, TProperty>(_propertyName, message));


    public RuleBuilder<TModel, TProperty> IsNull(string message = null)
        => AddValidator(new IsNullValidator<TModel, TProperty>(_propertyName, message));


    public RuleBuilder<TModel, TProperty> NotNull(string message = null)
        => AddValidator(new NotNullValidator<TModel, TProperty>(_propertyName, message));

    public RuleBuilder<TModel, TProperty> IsIn(IEnumerable<TProperty> list, string message = null)
        => AddValidator(new IsInValidator<TModel, TProperty>(_propertyName, list, message));


    public RuleBuilder<TModel, TProperty> NotIn(IEnumerable<TProperty> list, string message = null)
        => AddValidator(new NotInValidator<TModel, TProperty>(_propertyName, list, message));


    public RuleBuilder<TModel, TProperty> OneOf(params IEnumerable<TProperty> options)
        => AddValidator(new OneOfValidator<TModel, TProperty>(_propertyName, options));


    public RuleBuilder<TModel, TProperty> NoneOf(params IEnumerable<TProperty> options)
        => AddValidator(new NoneOfValidator<TModel, TProperty>(_propertyName, options));


    public RuleBuilder<TModel, TProperty> IsInEnum(string message = null)
        => AddValidator(new IsInEnumValidator<TModel, TProperty>(_propertyName, message));
}
internal sealed class MustValidator<TModel, TProperty>(string propertyName, Func<TProperty, bool> func, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    private readonly Func<TProperty, bool> _func = func;
    public string? Validate<T>(T value)
        => (value is TProperty v && !_func(v)) ? _message ?? $"{_propertyName} is not valid." : null;
}

internal sealed class CustomValidator<TModel, TProperty>(string propertyName, Func<bool> func, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    private readonly Func<bool> _func = func;
    public string? Validate<T>(T value)
        => (!_func.Invoke()) ? _message ?? $"{_propertyName} is not valid." : null;
}

internal sealed class SetValidatorValidator<TModel, TProperty>(string propertyName, ValidatorBuilder<TModel> parent, ValidatorBuilder<TProperty> validator, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    private readonly ValidatorBuilder<TProperty> _validator = validator;
    private readonly ValidatorBuilder<TModel> _parent = parent;
    public string? Validate<T>(T value)
    {
        if (value is TProperty property)
            _parent.Errors.AddRange(_validator.Validate(property).ErrorMessages);

        return null;
    }
}

internal sealed class EqualValidator<TModel, TProperty>(string propertyName, TProperty expected, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    private readonly TProperty _expected = expected;
    public string? Validate<T>(T value)
        => (!Equals(value, _expected)) ? _message ?? $"{_propertyName} must be equal to {_expected}." : null;
}

internal sealed class NotEqualValidator<TModel, TProperty>(string propertyName, TProperty unexpected, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    private readonly TProperty _unexpected = unexpected;
    public string? Validate<T>(T value)
        => (Equals(value, _unexpected)) ? _message ?? $"{_propertyName} must not be equal to {_unexpected}." : null;
}

internal sealed class IsEmptyValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    public string? Validate<T>(T value)
    {
        var isEmpty = value switch
        {
            null => true,
            string str => string.IsNullOrWhiteSpace(str),
            System.Collections.IEnumerable enumerable => !enumerable.Cast<object>().Any(),
            _ => false
        };
        if (!isEmpty)
            return _message ?? $"{_propertyName} must be empty";
        return null;
    }
}

internal sealed class NotEmptyValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    public string? Validate<T>(T value)
    {
        var isEmpty = value switch
        {
            null => true,
            string str => string.IsNullOrWhiteSpace(str),
            System.Collections.IEnumerable enumerable => !enumerable.Cast<object>().Any(),
            _ => false
        };
        if (isEmpty)
            return _message ?? $"{_propertyName} cannot be empty";
        return null;
    }
}

internal sealed class IsNullValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    public string? Validate<T>(T value)
        => value is not null ? _message ?? $"{_propertyName} must be null" : null;
}

internal sealed class NotNullValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    public string? Validate<T>(T value)
        => value is null ? _message ?? $"{_propertyName} cannot be null" : null;
}

internal sealed class IsInValidator<TModel, TProperty>(string propertyName, IEnumerable<TProperty> list, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    private readonly IEnumerable<TProperty> _list = list;
    public string? Validate<T>(T value)
        => (value is TProperty v && !_list.Contains(v)) ? _message ?? $"{_propertyName} must be one of: {string.Join(", ", _list)}." : null;
}

internal sealed class NotInValidator<TModel, TProperty>(string propertyName, IEnumerable<TProperty> list, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    private readonly IEnumerable<TProperty> _list = list;
    public string? Validate<T>(T value)
        => (value is TProperty v && _list.Contains(v)) ? _message ?? $"{_propertyName} must not be one of: {string.Join(", ", _list)}." : null;
}

internal sealed class OneOfValidator<TModel, TProperty>(string propertyName, params IEnumerable<TProperty> options)
    : IValidator
{
    private readonly string _propertyName = propertyName;
    private readonly IEnumerable<TProperty> _options = options;

    public string? Validate<T>(T value)
        => (value is TProperty v && !_options.Contains(v)) ? $"{_propertyName} must be one of the following values: {string.Join(", ", _options)}" : null;
}

internal sealed class NoneOfValidator<TModel, TProperty>(string propertyName, params IEnumerable<TProperty> options)
    : IValidator
{
    private readonly string _propertyName = propertyName;
    private readonly IEnumerable<TProperty> _options = options;

    public string? Validate<T>(T value)
        => (value is TProperty v && _options.Contains(v)) ? $"{_propertyName} must not be one of the following values: {string.Join(", ", _options)}" : null;
}

internal sealed class IsInEnumValidator<TModel, TProperty>(string propertyName, string? message)
     : BaseValidator(propertyName, message), IValidator
{
    public string? Validate<T>(T value)
        => (!Enum.IsDefined(typeof(TProperty), value)) ? $"{_propertyName} must be a valid enum value." : null;
}
