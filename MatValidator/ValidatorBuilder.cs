using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace MatValidator;
public abstract class AbstractValidator<TModel> : ValidatorBuilder<TModel> { }


public class ValidatorBuilder<TModel>
{
    private readonly List<string> _validErrors;
    private readonly List<IValidatiorRule> _rules;

    public ValidatorBuilder()
    {
        _rules = [];
        _validErrors = [];
    }

    public ValidResult Validate(TModel model)
    {
        foreach (var rule in _rules)
            _validErrors.AddRange(rule.Validate(model));

        return new ValidResult(CollectionsMarshal.AsSpan(_validErrors));
    }

    public RuleBuilder<TModel, TProperty> RuleFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
        if (expression.Body is not MemberExpression memberExpr)
            throw new ArgumentException("Expression must be a property access", nameof(expression));

        var propertyName = memberExpr.Member.Name;
        var func = expression.Compile();

        var rule = new RuleBuilder<TModel, TProperty>(this, propertyName, func);

        _rules.Add(rule);

        return rule;
    }

    internal void AddError(string error)
    {
        _validErrors.Add(error);
    }

}
