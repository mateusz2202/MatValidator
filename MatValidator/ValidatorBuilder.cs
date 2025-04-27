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

    public ValidResult Validate(object instance)
    {
        if (instance is TModel model)
        {
            ValidResult.ErrorMessages.AddRange(_rules.SelectMany(rule => rule.Validate(model)));
            return ValidResult;
        }

        throw new InvalidOperationException($"Invalid model type: expected {typeof(TModel).Name}, got {instance.GetType().Name}");
    }


    public RuleBuilder<TModel, TProperty> RuleFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
        var rule = new RuleBuilder<TModel, TProperty>(this, expression);

        _rules.Add(rule);

        return rule;
    }

}
