using MatValidator.Utils;
using System.Linq.Expressions;

namespace MatValidator;
public abstract class AbstractValidator<TModel> : ValidatorBuilder<TModel> { }


public class ValidatorBuilder<TModel>
{
    public List<string> Errors { get; init; }
    private readonly List<IValidatorRule> _rules;

    public ValidatorBuilder()
    {
        _rules = [];
        Errors = [];
    }

    public ValidResult Validate(TModel model)
    {
        var errors = _rules.SelectMany(rule => rule.Validate(model)).ToArray();
        var combinedErrors = Errors.Concat(errors).ToArray();
        return new ValidResult(combinedErrors);
    }


    public RuleBuilder<TModel, TProperty> RuleFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
        var rule = new RuleBuilder<TModel, TProperty>(this, expression);

        _rules.Add(rule);

        return rule;
    }

}
