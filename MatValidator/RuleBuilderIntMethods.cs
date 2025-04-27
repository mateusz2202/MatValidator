namespace MatValidator;
public sealed partial class RuleBuilder<TModel, TProperty> : IValidatorRule
{
    internal RuleBuilder<TModel, TProperty> Range(int min, int max, string message = null)
       => AddValidator(new RangeValidator<TModel, TProperty>(_propertyName, min, max, message));
}

public static class IntRuleBuilderExtensions
{
    public static RuleBuilder<TModel, int> Range<TModel>(this RuleBuilder<TModel, int> builder, int min, int max, string message = null)
        => builder.Range(min, max, message);

    public static RuleBuilder<TModel, int?> Range<TModel>(this RuleBuilder<TModel, int?> builder, int min, int max, string message = null)
        => builder.Range(min, max, message);
}

internal sealed class RangeValidator<TModel, TProperty>(string propertyName, int min, int max, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    private readonly int _min = min;
    private readonly int _max = max;

    public string? Validate<T>(T value)
        => (value is int v && (v > _max || v < _min)) ? _message ?? $"{_propertyName} must be between {_min} and {_max}." : null;

}