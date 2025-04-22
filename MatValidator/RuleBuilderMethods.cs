namespace MatValidator;
public partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    public RuleBuilder<TModel, TProperty> Must(Func<TProperty, bool> func, string message = null)
    {

        _validators.Add((
            _nextCondition,
            value =>
            {
                if (!func(value))
                    return new ValidError(message ?? $"{_propertyName} is not valid.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> Custom(Func<bool> func, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (!func.Invoke())
                    return new ValidError(message ?? "Error valid.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> OverridePropertyName(string propertName)
    {
        _propertyName = propertName;

        return this;
    }

    public RuleBuilder<TModel, TProperty> SetValidator(ValidatorBuilder<TProperty> validator)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value == null) return null;

                var result = validator.Validate(value);

                foreach (var error in result.ErrorMessages)
                    _parent.AddError(new ValidError($"{_propertyName}.{error}"));

                return null;
            }
        ));

        return this;
    }

    public RuleBuilder<TModel, TProperty> Equal(TProperty expected, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (!Equals(value, expected))
                    return new ValidError(message ?? $"{_propertyName} must be equal to {expected}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> NotEqual(TProperty unexpected, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (Equals(value, unexpected))
                    return new ValidError(message ?? $"{_propertyName} must not be equal to {unexpected}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsEmpty(string message = null)
    {
        _validators.Add((
         _nextCondition,
         value =>
         {
             var isEmpty = value switch
             {
                 null => true,
                 string str => string.IsNullOrWhiteSpace(str),
                 System.Collections.IEnumerable enumerable => !enumerable.Cast<object>().Any(),
                 _ => false
             };

             if (!isEmpty)
                 return new ValidError(message ?? $"{_propertyName} must be empty");

             return null;
         }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> NotEmpty(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                var isEmpty = value switch
                {
                    null => true,
                    string str => string.IsNullOrWhiteSpace(str),
                    System.Collections.IEnumerable enumerable => !enumerable.Cast<object>().Any(),
                    _ => false
                };

                if (isEmpty)
                    return new ValidError(message ?? $"{_propertyName} cannot be empty");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsNull(string message = null)
    {
        _validators.Add((
            _nextCondition,
             value => value is not null
                    ? new ValidError(message ?? $"{_propertyName} must be null")
                    : null
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> NotNull(string message = null)
    {
        _validators.Add((
            _nextCondition,
             value => value is null
                    ? new ValidError(message ?? $"{_propertyName} cannot be null")
                    : null
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsIn(IEnumerable<TProperty> list, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (!list.Contains(value))
                    return new ValidError(message ?? $"{_propertyName} must be one of: {string.Join(", ", list)}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> NotIn(IEnumerable<TProperty> list, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (list.Contains(value))
                    return new ValidError(message ?? $"{_propertyName} must not be one of: {string.Join(", ", list)}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> OneOf(params IEnumerable<TProperty> options)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (!options.Contains(value))
                    return new ValidError($"{_propertyName} must be one of the following values: {string.Join(", ", options)}");

                return null;
            }
        ));

        _nextCondition = _ => true;
        return this;
    }

    public RuleBuilder<TModel, TProperty> NoneOf(params IEnumerable<TProperty> options)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (options.Contains(value))
                    return new ValidError($"{_propertyName} must not be one of the following values: {string.Join(", ", options)}");

                return null;
            }
        ));

        _nextCondition = _ => true;
        return this;
    }

    public RuleBuilder<TModel, TProperty> IsInEnum(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value == null || !Enum.IsDefined(typeof(TProperty), value))
                    return new ValidError(message ?? $"{_propertyName} must be a valid enum value.");

                return null;
            }
        ));

        _nextCondition = _ => true;
        return this;
    }
}
