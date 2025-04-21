using System.Linq.Expressions;

namespace MatValidator;
public abstract class AbstractValidator<TModel> : ValidatorBuilder<TModel> { }


public class ValidatorBuilder<TModel>
{
    private readonly List<IValidationRule<TModel>> _rules;
    private readonly List<ValidError> _validErrors;
    public ValidatorBuilder()
    {
        _rules = [];
        _validErrors = [];
    }

    public ValidResult Validate(TModel model)
    {
        foreach (var rule in _rules)
            rule.Validate(model);

        return new ValidResult([.. _validErrors.Select(x => x.ErrorMessage)]);
    }

    public RuleBuilder<TModel, TProperty> RuleFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
        if (expression.Body is not MemberExpression memberExpr)
            throw new ArgumentException("Expression must be a property access", nameof(expression));

        var propertyName = memberExpr.Member.Name;
        var func = expression.Compile();

        var rule = new RuleBuilder<TModel, TProperty>(this, func, propertyName);

        _rules.Add(rule);

        return rule;
    }

    internal void AddError(ValidError error)
    {
        _validErrors.Add(error);
    }
}
