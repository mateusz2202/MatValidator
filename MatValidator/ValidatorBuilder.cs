using System.Linq.Expressions;

namespace MatValidator;
public abstract class AbstractValidator<TModel> : ValidatorBuilder<TModel> { }


public class ValidatorBuilder<TModel>
{
    private ValidResult _result;
    private readonly List<IValidatiorRule> _rules;

    public ValidatorBuilder()
    {
        _rules = [];
        _result = new ValidResult([]);
    }

    public ValidResult Validate(TModel model)
    {
        var errors = _rules.SelectMany(rule => rule.Validate(model)).ToArray();

        Span<string> span1 = _result.ErrorMessages;
        Span<string> span2 = errors;

        string[] result = new string[span1.Length + span2.Length];
        span1.CopyTo(result);
        span2.CopyTo(result.AsSpan(span1.Length));
        _result = new ValidResult(result);

        return _result;
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

    internal void AddError(ValidResult validResult)
    {
        Span<string> span1 = _result.ErrorMessages;
        Span<string> span2 = validResult.ErrorMessages;
        string[] result = new string[span1.Length + span2.Length];
        span1.CopyTo(result);
        span2.CopyTo(result.AsSpan(span1.Length));
        _result = new ValidResult(result);
    }

}
