using System.Linq.Expressions;

namespace MatValidator;
public abstract class AbstractValidator<TModel> : ValidatorBuilder<TModel> { }

public class ValidatorBuilder<TModel>
{
    public readonly ValidResult ValidResult;
    private readonly List<IValidatorRule> _rules;

    public ValidatorBuilder()
    {
        _rules = [];
        ValidResult = new([]);
    }

    public ValidResult Validate(TModel model)
    {
        ValidResult.ErrorMessages.AddRange(_rules.SelectMany(rule => rule.Validate(model)));
        return ValidResult;
    }


    public RuleBuilder<TModel, TProperty> RuleFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
        var rule = new RuleBuilder<TModel, TProperty>(this, expression);

        _rules.Add(rule);

        return rule;
    }

}
