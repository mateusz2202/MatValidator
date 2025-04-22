namespace MatValidator;

internal interface IValidationRule<TModel>
{
    void Validate(TModel instance);
}

public partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    private readonly ValidatorBuilder<TModel> _parent;
    private Func<TModel, bool> _shouldValidate = _ => true;
    private Func<TModel, bool> _nextCondition = _ => true;
    private readonly Func<TModel, TProperty> _propertyFunc;
    private readonly List<(Func<TModel, bool> Condition, Func<TProperty, ValidError> Validator)> _validators;
    private string _propertyName;

    public RuleBuilder(ValidatorBuilder<TModel> parent, Func<TModel, TProperty> propertyFunc, string propertyName)
    {
        _parent = parent;
        _propertyFunc = propertyFunc;
        _propertyName = propertyName;
        _validators = [];
    }

    public void Validate(TModel instance)
    {
        if (!_shouldValidate(instance)) return;

        var value = _propertyFunc(instance);

        foreach (var (condition, validator) in _validators)
        {
            if (!condition(instance)) continue;

            var error = validator.Invoke(value);

            if (error is not null)
                _parent.AddError(error);
        }
    }
}