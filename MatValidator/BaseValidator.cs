namespace MatValidator;

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

