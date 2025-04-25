namespace MatValidator;
public sealed partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    public RuleBuilder<TModel, TProperty> Must(Func<TProperty, bool> func, string message = null)
        => AddValidator(value =>
        {
            if (value is TProperty p && !func(p))
                return message ?? $"{_propertyName} is not valid.";
            return null;
        });


    public RuleBuilder<TModel, TProperty> Custom(Func<bool> func, string message = null)
        => AddValidator(value =>
        {
            if (!func.Invoke())
                return message ?? $"{_propertyName} is not valid.";
            return null;
        });


    public RuleBuilder<TModel, TProperty> OverridePropertyName(string propertName)
    {
        _propertyName = propertName;

        return this;
    }

    public RuleBuilder<TModel, TProperty> SetValidator(ValidatorBuilder<TProperty> validator)
        => AddValidator(value =>
        {
            if (value == null) return null;
            if (value is TProperty p)
            {
                var result = validator.Validate(p);
                foreach (var error in result.ErrorMessages)
                    _parent.AddError($"{_propertyName}.{error}");
            }
            return null;
        });


    public RuleBuilder<TModel, TProperty> Equal(TProperty expected, string message = null)
        => AddValidator(value =>
        {
            if (value is TProperty p && !Equals(p, expected))
                return message ?? $"{_propertyName} must be equal to {expected}.";
            return null;
        });


    public RuleBuilder<TModel, TProperty> NotEqual(TProperty unexpected, string message = null)
        => AddValidator(value =>
        {
            if (value is TProperty p && Equals(p, unexpected))
                return message ?? $"{_propertyName} must not be equal to {unexpected}.";
            return null;
        });


    public RuleBuilder<TModel, TProperty> IsEmpty(string message = null)
        => AddValidator(value =>
        {
            var isEmpty = value switch
            {
                null => true,
                string str => string.IsNullOrWhiteSpace(str),
                System.Collections.IEnumerable enumerable => !enumerable.Cast<object>().Any(),
                _ => false
            };
            if (!isEmpty)
                return message ?? $"{_propertyName} must be empty";
            return null;
        });


    public RuleBuilder<TModel, TProperty> NotEmpty(string message = null)
        => AddValidator(value =>
        {
            var isEmpty = value switch
            {
                null => true,
                string str => string.IsNullOrWhiteSpace(str),
                System.Collections.IEnumerable enumerable => !enumerable.Cast<object>().Any(),
                _ => false
            };
            if (isEmpty)
                return message ?? $"{_propertyName} cannot be empty";
            return null;
        });

    public RuleBuilder<TModel, TProperty> IsNull(string message = null)
        => AddValidator(value => value is not null ? message ?? $"{_propertyName} must be null" : null);


    public RuleBuilder<TModel, TProperty> NotNull(string message = null)
        => AddValidator(value => value is null ? message ?? $"{_propertyName} cannot be null" : null);


    public RuleBuilder<TModel, TProperty> IsIn(IEnumerable<TProperty> list, string message = null)
        => AddValidator(value =>
        {
            if (value is TProperty p && !list.Contains(p))
                return message ?? $"{_propertyName} must be one of: {string.Join(", ", list)}.";
            return null;
        });


    public RuleBuilder<TModel, TProperty> NotIn(IEnumerable<TProperty> list, string message = null)
        => AddValidator(value =>
        {
            if (value is TProperty p && list.Contains(p))
                return message ?? $"{_propertyName} must not be one of: {string.Join(", ", list)}.";
            return null;
        });


    public RuleBuilder<TModel, TProperty> OneOf(params IEnumerable<TProperty> options)
        => AddValidator(value =>
        {
            if (value is TProperty p && !options.Contains(p))
                return $"{_propertyName} must be one of the following values: {string.Join(", ", options)}";
            return null;
        });

    public RuleBuilder<TModel, TProperty> NoneOf(params IEnumerable<TProperty> options)
        => AddValidator(value =>
        {
            if (value is TProperty p && options.Contains(p))
                return $"{_propertyName} must not be one of the following values: {string.Join(", ", options)}";
            return null;
        });


    public RuleBuilder<TModel, TProperty> IsInEnum(string message = null)
        => AddValidator(value =>
        {
            if (value is TProperty p && !Enum.IsDefined(typeof(TProperty), p))
                return message ?? $"{_propertyName} must be a valid enum value.";
            return null;
        });
}
