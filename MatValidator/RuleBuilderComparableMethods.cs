namespace MatValidator;
public partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    internal RuleBuilder<TModel, TProperty> GreaterThan(TProperty threshold, string message = null)
        => AddValidator(value =>
        {
            if (value is IComparable comparable && comparable.CompareTo(threshold) <= 0)
                return message ?? $"{_propertyName} must be greater than {threshold}.";

            return null;
        });


    internal RuleBuilder<TModel, TProperty> LessThan(TProperty threshold, string message = null)
        => AddValidator(value =>
        {
            if (value is IComparable comparable && comparable.CompareTo(threshold) >= 0)
                return message ?? $"{_propertyName} must be less than {threshold}.";

            return null;
        });


    internal RuleBuilder<TModel, TProperty> GreaterThanOrEqual(TProperty threshold, string message = null)
        => AddValidator(value =>
        {
            if (value is IComparable comparable && comparable.CompareTo(threshold) < 0)
                return message ?? $"{_propertyName} must be greater than or equal to {threshold}.";

            return null;
        });


    internal RuleBuilder<TModel, TProperty> LessThanOrEqual(TProperty threshold, string message = null)
        => AddValidator(value =>
        {
            if (value is IComparable comparable && comparable.CompareTo(threshold) > 0)
                return message ?? $"{_propertyName} must be less than or equal to {threshold}.";

            return null;
        });

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