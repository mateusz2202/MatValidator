using System.Data;
using System.Linq.Expressions;

namespace MatValidator;

public record ValidResult(List<string> ErrorMessages)
{
    public bool IsValid => ErrorMessages.Count == 0;
}
public record ValidError(string ErrorMessage);

public abstract class AbstractValidator<TModel> : ValidatorBuilder<TModel>
{

}


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
public interface IValidationRule<TModel>
{
    void Validate(TModel instance);
}

public class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    private readonly ValidatorBuilder<TModel> _parent;
    private Func<TModel, bool> _shouldValidate = _ => true;
    private Func<TModel, bool> _nextCondition = _ => true;
    private readonly Func<TModel, TProperty> _propertyFunc;
    private readonly List<(Func<TModel, bool> Condition, Func<TProperty, ValidError> Validator)> _validators;
    private string _propertyName;

    public RuleBuilder(ValidatorBuilder<TModel> parent, Func<TModel, TProperty> propertyFunc, string propertyName)
    {
        _parent = parent;
        _propertyFunc = propertyFunc;
        _propertyName = propertyName;
        _validators = [];
    }

    public void Validate(TModel instance)
    {
        if (!_shouldValidate(instance)) return;

        var value = _propertyFunc(instance);

        foreach (var (condition, validator) in _validators)
        {
            if (!condition(instance)) continue;

            var error = validator.Invoke(value);

            if (error is not null)
                _parent.AddError(error);
        }
    }

    public RuleBuilder<TModel, TProperty> When(Func<TModel, bool> condition)
    {
        _shouldValidate = condition ?? (_ => true);
        return this;
    }

    public RuleBuilder<TModel, TProperty> Unless(Func<TModel, bool> predicate)
    {
        _nextCondition = model => !predicate(model);
        return this;
    }

    public RuleBuilder<TModel, TProperty> If(Func<TModel, bool> condition)
    {
        _nextCondition = condition ?? (_ => true);
        return this;
    }

    public RuleBuilder<TModel, TProperty> OverridePropertyName(string propertName)
    {
        _propertyName = propertName;
        return this;
    }

    public RuleBuilder<TModel, TProperty> SetValidator(ValidatorBuilder<TProperty> validator)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value == null) return null;

                var result = validator.Validate(value);

                foreach (var error in result.ErrorMessages)
                {
                    return new ValidError($"{_propertyName}.{error}");
                }

                return null;
            }
        ));

        return this;
    }


    public RuleBuilder<TModel, TProperty> NotEmpty(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                bool isValid = typeof(TProperty) == typeof(string) ? !string.IsNullOrWhiteSpace(value as string) : value is not null;

                if (!isValid)
                    return new ValidError(message ?? $"{_propertyName} is required");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> Range(int min, int max, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is int v && (v > max || v < min))
                    return new ValidError(message ?? $"{_propertyName} must be between {min} and {max} .");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> Max(int max, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is int v && v > max)
                    return new ValidError(message ?? $"{_propertyName} must be greater {max} .");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> Min(int min, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is int v && v < min)
                    return new ValidError(message ?? $"{_propertyName} must be less {min} .");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> Length(int min, int max, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is string str && (str.Length > max || str.Length < min))
                    return new ValidError(message ?? $"{_propertyName} length must be between {min} and {max} characters.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> MaxLength(int max, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is string str && str.Length > max)
                    return new ValidError(message ?? $"{_propertyName} length must be greater {max} characters.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> MinLength(int min, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is string str && str.Length < min)
                    return new ValidError(message ?? $"{_propertyName} length must be less {min} characters.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsEmail(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is string str && !str.Contains('@'))
                    return new ValidError(message ?? $"{_propertyName} is not a valid email");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> Custom(Func<TProperty, bool> func, string message = null)
    {

        _validators.Add((
            _nextCondition,
            value =>
            {
                if (!func(value))
                    return new ValidError(message ?? $"{_propertyName} is not valid.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> Custom(Func<bool> func, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (!func.Invoke())
                    return new ValidError(message ?? "Error valid.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }
}
