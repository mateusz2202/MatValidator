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
    private readonly List<string> _validErrors;
    private readonly Dictionary<int, List<ValidatorRule<TModel>>> _validatorMap;
    private int _ruleId;
    private readonly Dictionary<int, (IValidationRule<TModel>, Func<TModel, object>)> _rules;
    public ValidatorBuilder()
    {
        _ruleId = 0;
        _rules = [];
        _validErrors = [];
        _validatorMap = [];
    }

    public ValidResult Validate(TModel model)
    {
        var errors = GetErrors(model).Concat(_validErrors);

        if (_validErrors.Count == 0)
            return new ValidResult([.. errors]);

        return new ValidResult([.. errors.Concat(_validErrors)]);
    }

    public IEnumerable<string> GetErrors(TModel model)
    {
        foreach (var kvp in _validatorMap)
        {
            var rule = _rules[kvp.Key];
            if (!rule.Item1.ShouldValidate(model)) continue;

            var value = rule.Item2(model);

            foreach (var v in kvp.Value)
            {
                if (!v.Condition(model)) continue;

                var error = v.Validator(value);
                if (error is not null)
                    yield return error;
            }
        }
    }


    public RuleBuilder<TModel, TProperty> RuleFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
        if (expression.Body is not MemberExpression memberExpr)
            throw new ArgumentException("Expression must be a property access", nameof(expression));

        var propertyName = memberExpr.Member.Name;
        var func = expression.Compile();

        var rule = new RuleBuilder<TModel, TProperty>(this, propertyName, _ruleId);

        _rules.Add(_ruleId++, (rule, model => func(model)!));

        return rule;
    }

    internal void AddError(string error)
    {
        _validErrors.Add(error);
    }

    internal void AddValidator(ValidatorRule<TModel> validator)
    {
        if (!_validatorMap.TryGetValue(validator.RuleId, out var list))
        {
            list = [];
            _validatorMap[validator.RuleId] = list;
        }
        list.Add(validator);
    }
}
