namespace MatValidator;
public sealed partial class RuleBuilder<TModel, TProperty> : IValidatiorRule
{
    internal RuleBuilder<TModel, TProperty> IsTrue(string? message = null)
        => AddValidator(new IsTrueValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsFalse(string? message = null)
        => AddValidator(new IsFalseValidator<TModel, TProperty>(_propertyName, message));
}

public static class BoolRuleBuilderExtensions
{
    public static RuleBuilder<TModel, bool> IsTrue<TModel>(this RuleBuilder<TModel, bool> builder, string? message = null)
        => builder.IsTrue(message);

    public static RuleBuilder<TModel, bool> IsFalse<TModel>(this RuleBuilder<TModel, bool> builder, string? message = null)
        => builder.IsFalse(message);
}

internal sealed class IsTrueValidator<TModel, TProperty>(string propertyName, string? message)
     : BaseValidator(propertyName, message), IValidator
{
    public string? Validate<T>(T value)
        => value is bool b && !b ? _message ?? $"{_propertyName} must be true." : null;
}

internal sealed class IsFalseValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidator
{
    public string? Validate<T>(T value)
        => value is bool b && b ? _message ?? $"{_propertyName} must be false." : null;

}
