using System.Linq.Expressions;

namespace MatValidator;
public abstract class AbstractValidator<TModel> : ValidatorBuilder<TModel> { }

public struct ValidatorRule<TModel>
{
    public Func<TModel, bool> Condition { get; set; }
    public Func<object, string> Validator { get; set; }
    public int RuleId { get; set; }

    public ValidatorRule(Func<TModel, bool> condition, Func<object, string> validator, int ruleId)
    {
        Condition = condition;
        Validator = validator;
        RuleId = ruleId;
    }
}

public class ValidatorBuilder<TModel>
{
    private readonly List<IValidationRule<TModel>> _rules;
    private readonly List<string> _validErrors;
    private readonly List<ValidatorRule<TModel>> _validators;
    public ValidatorBuilder()
    {
        _rules = [];
        _validErrors = [];
        _validators = [];
    }

    public ValidResult Validate(TModel model)
    {
        var result = GetErrors(model).ToList();
        result.AddRange(_validErrors);
        return new ValidResult(result);
    }

    public IEnumerable<string> GetErrors(TModel model)
    {
        var rules = _validators.GroupBy(x => x.RuleId).ToList();
        foreach (var r in rules)
        {
            var rule = _rules.FirstOrDefault(x => x.Id == r.Key);
            if (!rule.ShouldValidate(model)) continue;
            var value = _funcValueRule[r.Key](model);
            foreach (var v in r)
            {
                if (!v.Condition(model)) continue;

                var error = v.Validator.Invoke(value);

                if (error is not null)
                    yield return error;
            }
        }
    }
    private int _ruleId = 0;
    private Dictionary<int, Func<TModel, object>> _funcValueRule = [];

    public RuleBuilder<TModel, TProperty> RuleFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
        if (expression.Body is not MemberExpression memberExpr)
            throw new ArgumentException("Expression must be a property access", nameof(expression));

        var propertyName = memberExpr.Member.Name;
        var func = expression.Compile();

        _funcValueRule.Add(_ruleId, model => func(model)!);
        var rule = new RuleBuilder<TModel, TProperty>(this, propertyName, _ruleId++);

        _rules.Add(rule);

        return rule;
    }

    internal void AddError(string error)
    {
        _validErrors.Add(error);
    }

    internal void AddValidator(ValidatorRule<TModel> validator)
    {
        _validators.Add(validator);
    }
}
