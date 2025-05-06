using System.Linq.Expressions;

namespace MatValidator;
public abstract class AbstractValidator<TModel> : ValidatorBuilder<TModel> { }

public class ValidatorBuilder<TModel> : IValidator
{
    public readonly ValidResult ValidResult;
    private readonly List<IValidatorRule> _rules;

    public ValidatorBuilder()
    {
        _rules = [];
        ValidResult = new([]);
    }

    public async Task<ValidResult> ValidateAsync(object instance, CancellationToken cancellationToken = default)
    {
        if (instance is not TModel model)
            throw new InvalidOperationException($"Invalid model type: expected {typeof(TModel).Name}, got {instance.GetType().Name}");


        foreach (var rule in _rules)
        {
            await foreach (var error in rule.ValidateAsync(model, cancellationToken))
            {
                if (!string.IsNullOrWhiteSpace(error))
                    ValidResult.ErrorMessages.Add(error);
            }
        }

        return ValidResult;
    }


    public RuleBuilder<TModel, TProperty> RuleFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
        var rule = new RuleBuilder<TModel, TProperty>(this, expression);

        _rules.Add(rule);

        return rule;
    }


    public void Include<T>(ValidatorBuilder<T> validator)
    {
        _rules.AddRange(validator._rules);
    }

}
