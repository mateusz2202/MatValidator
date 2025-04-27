namespace MatValidator;
public interface IValidator
{
    ValidResult Validate(object instance);
}
public interface IValidatorProperty
{
    string? Validate<T>(T value);
}
public interface IValidatorRule
{
    IEnumerable<string> Validate<T>(T model);
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

