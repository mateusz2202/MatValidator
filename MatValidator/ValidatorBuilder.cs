using System.Linq.Expressions;
using System.Runtime.InteropServices;

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
public readonly struct ValidationRuleEntry<TModel>
{
    public IValidationRule<TModel> Rule { get; init; }
    public Func<TModel, object> Accessor { get; init; }

    public List<ValidatorRule<TModel>> Validators { get; init; }
    public ValidationRuleEntry(IValidationRule<TModel> rule, Func<TModel, object> accessor)
    {
        Rule = rule;
        Accessor = accessor;
        Validators = [];
    }
}

public class ValidatorBuilder<TModel>
{
    private readonly List<string> _validErrors;
    private int _ruleId;
    private readonly Dictionary<int, ValidationRuleEntry<TModel>> _rules;
    public ValidatorBuilder()
    {
        _ruleId = 0;
        _rules = [];
        _validErrors = [];
    }

    public ValidResult Validate(TModel model)
    {
        var errors = GetErrors(model).Concat(_validErrors);

        if (_validErrors.Count == 0)
            return new ValidResult(CollectionsMarshal.AsSpan(errors.ToList()));

        return new ValidResult(CollectionsMarshal.AsSpan(errors.Concat(_validErrors).ToList()));
    }

    private IEnumerable<string> GetErrors(TModel model)
    {
        foreach (var rule in _rules)
        {
            if (!rule.Value.Rule.ShouldValidate(model)) continue;

            var value = rule.Value.Accessor(model);

            foreach (var v in rule.Value.Validators)
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

        _rules.Add(_ruleId++, new(rule, model => func(model)!));

        return rule;
    }

    internal void AddError(string error)
    {
        _validErrors.Add(error);
    }

    internal void AddValidator(ValidatorRule<TModel> validator)
    {
        if (_rules.TryGetValue(validator.RuleId, out var rule))
            rule.Validators.Add(validator);

    }
}
