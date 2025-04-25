namespace MatValidator;
public sealed partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    internal RuleBuilder<TModel, TProperty> IsTrue(string message = null)
        => AddValidator(value =>
        {
            if (value is bool b && !b)
                return message ?? $"{_propertyName} must be true.";

            return null;
        });


    internal RuleBuilder<TModel, TProperty> IsFalse(string message = null)
        => AddValidator(value =>
        {
            if (value is bool b && b)
                return message ?? $"{_propertyName} must be false.";

            return null;
        });
}


public static class BoolRuleBuilderExtensions
{
    public static RuleBuilder<TModel, bool> IsTrue<TModel>(this RuleBuilder<TModel, bool> builder, string message = null)
        => builder.IsTrue(message);

    public static RuleBuilder<TModel, bool> IsFalse<TModel>(this RuleBuilder<TModel, bool> builder, string message = null)
        => builder.IsFalse(message);
}