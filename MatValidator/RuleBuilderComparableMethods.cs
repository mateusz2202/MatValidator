namespace MatValidator;
public sealed partial class RuleBuilder<TModel, TProperty> : IValidatorRule
{
    internal RuleBuilder<TModel, TProperty> GreaterThan(TProperty threshold, string message = null)
        => AddValidator(new GreaterThanValidator<TModel, TProperty>(_propertyName, threshold, message));

    internal RuleBuilder<TModel, TProperty> LessThan(TProperty threshold, string message = null)
        => AddValidator(new LessThanValidator<TModel, TProperty>(_propertyName, threshold, message));

    internal RuleBuilder<TModel, TProperty> GreaterThanOrEqual(TProperty threshold, string message = null)
         => AddValidator(new GreaterThanOrEqualValidator<TModel, TProperty>(_propertyName, threshold, message));

    internal RuleBuilder<TModel, TProperty> LessThanOrEqual(TProperty threshold, string message = null)
        => AddValidator(new LessThanOrEqualValidator<TModel, TProperty>(_propertyName, threshold, message));

}

public static class IComparableRuleBuilderExtensions
{
    public static RuleBuilder<TModel, TProperty> GreaterThan<TModel, TProperty>
        (this RuleBuilder<TModel, TProperty> builder, TProperty threshold, string message = null)
        where TProperty : IComparable
        => builder.GreaterThan(threshold, message);

    public static RuleBuilder<TModel, TProperty> LessThan<TModel, TProperty>
        (this RuleBuilder<TModel, TProperty> builder, TProperty threshold, string message = null)
        where TProperty : IComparable
        => builder.LessThan(threshold, message);

    public static RuleBuilder<TModel, TProperty> GreaterThanOrEqual<TModel, TProperty>
        (this RuleBuilder<TModel, TProperty> builder, TProperty threshold, string message = null)
        where TProperty : IComparable
        => builder.GreaterThanOrEqual(threshold, message);

    public static RuleBuilder<TModel, TProperty> LessThanOrEqual<TModel, TProperty>
        (this RuleBuilder<TModel, TProperty> builder, TProperty threshold, string message = null)
        where TProperty : IComparable
        => builder.LessThanOrEqual(threshold, message);


    public static RuleBuilder<TModel, TProperty?> GreaterThan<TModel, TProperty>
        (this RuleBuilder<TModel, TProperty?> builder, TProperty? threshold, string message = null)
        where TProperty : struct, IComparable<TProperty>, IComparable
        => builder.GreaterThan(threshold, message);

    public static RuleBuilder<TModel, TProperty?> LessThan<TModel, TProperty>
        (this RuleBuilder<TModel, TProperty?> builder, TProperty? threshold, string message = null)
        where TProperty : struct, IComparable<TProperty>, IComparable
        => builder.LessThan(threshold, message);

    public static RuleBuilder<TModel, TProperty?> GreaterThanOrEqual<TModel, TProperty>
        (this RuleBuilder<TModel, TProperty?> builder, TProperty? threshold, string message = null)
        where TProperty : struct, IComparable<TProperty>, IComparable
        => builder.GreaterThanOrEqual(threshold, message);

    public static RuleBuilder<TModel, TProperty?> LessThanOrEqual<TModel, TProperty>
        (this RuleBuilder<TModel, TProperty?> builder, TProperty? threshold, string message = null)
        where TProperty : struct, IComparable<TProperty>, IComparable
        => builder.LessThanOrEqual(threshold, message);
}

internal sealed class GreaterThanValidator<TModel, TProperty>(string propertyName, TProperty threshold, string? message)
    : ComparisonValidatorBase<TProperty>(propertyName, threshold, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => value is IComparable comparable && comparable.CompareTo(_threshold) <= 0 ? _message ?? $"{_propertyName} must be greater than {_threshold}." : null;
}

internal sealed class LessThanValidator<TModel, TProperty>(string propertyName, TProperty threshold, string? message)
    : ComparisonValidatorBase<TProperty>(propertyName, threshold, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => value is IComparable comparable && comparable.CompareTo(_threshold) >= 0 ? _message ?? $"{_propertyName} must be less than {_threshold}." : null;
}

internal sealed class GreaterThanOrEqualValidator<TModel, TProperty>(string propertyName, TProperty threshold, string? message)
    : ComparisonValidatorBase<TProperty>(propertyName, threshold, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => value is IComparable comparable && comparable.CompareTo(_threshold) < 0 ? _message ?? $"{_propertyName} must be greater than or equal to {_threshold}." : null;
}

internal sealed class LessThanOrEqualValidator<TModel, TProperty>(string propertyName, TProperty threshold, string? message)
    : ComparisonValidatorBase<TProperty>(propertyName, threshold, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => value is IComparable comparable && comparable.CompareTo(_threshold) > 0 ? _message ?? $"{_propertyName} must be less than or equal to {_threshold}." : null;
}
