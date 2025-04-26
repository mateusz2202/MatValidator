using MatValidator.Utils;
using System.Linq.Expressions;

namespace MatValidator;
public interface IValidator
{
    string? Validate<T>(T value);
}
public interface IValidatiorRule
{
    IEnumerable<string> Validate<T>(T model);
}

public sealed partial class RuleBuilder<TModel, TProperty> : IValidatiorRule
{
    private readonly ValidatorBuilder<TModel> _parent;
    private string _propertyName;

    public Predicate<TModel> ShouldValidate { get; private set; } = _ => true;
    public Predicate<TModel> NextCondition { get; private set; } = _ => true;

    private readonly List<IValidator> _validators;
    private readonly Expression<Func<TModel, TProperty>> _accessor;

    public RuleBuilder(ValidatorBuilder<TModel> parent, string propertyName, Expression<Func<TModel, TProperty>> accessor)
    {
        _parent = parent;
        _propertyName = propertyName;
        _accessor = accessor;
        _validators = [];
    }

    public RuleBuilder<TModel, TProperty> OverridePropertyName(string propertName)
    {
        _propertyName = propertName;

        return this;
    }

    public RuleBuilder<TModel, TProperty> AddValidator(IValidator validator)
    {
        _validators.Add(validator);
        NextCondition = _ => true;
        return this;
    }

    public IEnumerable<string> Validate<T>(T model)
    {
        if (model is not TModel typedModel || !ShouldValidate(typedModel))
            yield break;

        foreach (var validator in _validators)
        {
            if (!NextCondition(typedModel))
            {
                NextCondition = _ => true;
                continue;
            }

            var value = typedModel.GetPropertyValue(_accessor);
            var error = validator.Validate(value);
            if (error is not null)
                yield return error;
        }
    }
}