namespace MatValidator;
public partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    internal RuleBuilder<TModel, TProperty> IsTrue(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is bool b && !b)
                    return new ValidError(message ?? $"{_propertyName} must be true.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsFalse(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is bool b && b)
                    return new ValidError(message ?? $"{_propertyName} must be false.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }
}


public static class BoolRuleBuilderExtensions
{
    public static RuleBuilder<TModel, bool> IsTrue<TModel>(this RuleBuilder<TModel, bool> builder, string message = null)
        => builder.IsTrue(message);

    public static RuleBuilder<TModel, bool> IsFalse<TModel>(this RuleBuilder<TModel, bool> builder, string message = null)
        => builder.IsFalse(message);
}