namespace MatValidator;
public partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    internal RuleBuilder<TModel, TProperty> Range(int min, int max, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is int v && (v > max || v < min))
                    return new ValidError(message ?? $"{_propertyName} must be between {min} and {max} .");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }
}

public static class IntRuleBuilderExtensions
{
    public static RuleBuilder<TModel, int> Range<TModel>(this RuleBuilder<TModel, int> builder, int min, int max, string message = null)
        => builder.Range(min, max, message);

    public static RuleBuilder<TModel, int?> Range<TModel>(this RuleBuilder<TModel, int?> builder, int min, int max, string message = null)
        => builder.Range(min, max, message);
}
