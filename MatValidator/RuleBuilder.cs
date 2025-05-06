using MatValidator.Utils;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MatValidator;

public sealed partial class RuleBuilder<TModel, TProperty> : IValidatorRule
{
    private readonly ValidatorBuilder<TModel> _parent;
    private string _propertyName;

    public Predicate<TModel> ShouldValidate { get; private set; } = _ => true;
    public Predicate<TModel> NextCondition { get; private set; } = _ => true;

    private readonly List<IValidatorBaseProperty> _validators;
    private readonly Expression<Func<TModel, TProperty>> _accessor;

    public RuleBuilder(ValidatorBuilder<TModel> parent, Expression<Func<TModel, TProperty>> accessor)
    {
        _parent = parent;
        _propertyName = accessor.GetPropertyName();
        _accessor = accessor;
        _validators = [];
    }

    public RuleBuilder<TModel, TProperty> OverridePropertyName(string propertName)
    {
        _propertyName = propertName;

        return this;
    }

    public RuleBuilder<TModel, TProperty> AddValidator(IValidatorBaseProperty validator)
    {
        _validators.Add(validator);
        NextCondition = _ => true;
        return this;
    }

    public async IAsyncEnumerable<string> ValidateAsync<T>(T model, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (model is not TModel typedModel || !ShouldValidate(typedModel))
            yield break;


        for (int i = 0; i < _validators.Count; i++)
        {
            if (!NextCondition(typedModel))
            {
                NextCondition = _ => true;
                continue;
            }

            string error = null;

            if (_validators[i] is IValidatorProperty validator)
            {
                error = validator.Validate(typedModel.GetPropertyValue(_accessor));
            }
            else if (_validators[i] is IValidatorAsyncProperty asyncValidator)
            {
                error = await asyncValidator.ValidateAsync(typedModel.GetPropertyValue(_accessor), cancellationToken);
            }

            if (error is not null)
                yield return error;
        }
    }
}