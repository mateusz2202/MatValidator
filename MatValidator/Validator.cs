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
    private bool _shouldValidate = true;
    private readonly Func<TModel, TProperty> _propertyFunc;
    private readonly List<Action<TProperty>> _validActions;
    private readonly string _propertyName;

    public RuleBuilder(ValidatorBuilder<TModel> parent, Func<TModel, TProperty> propertyFunc, string propertyName)
    {
        _parent = parent;
        _propertyFunc = propertyFunc;
        _propertyName = propertyName;
        _validActions = [];
    }

    public void Validate(TModel instance)
    {
        var value = _propertyFunc(instance);

        foreach (var action in _validActions)
            action.Invoke(value);
    }

    public RuleBuilder<TModel, TProperty> When(Func<bool> condition)
    {
        _shouldValidate = condition();
        return this;
    }


    public RuleBuilder<TModel, TProperty> NotEmpty(string message = null)
    {
        if (!_shouldValidate) return this;

        _validActions.Add(value =>
        {
            bool isValid = typeof(TProperty) == typeof(string) ? !string.IsNullOrWhiteSpace(value as string) : value is not null;

            if (!isValid)
                _parent.AddError(new ValidError(message ?? $"{_propertyName} is required"));
        });

        return this;
    }

    public RuleBuilder<TModel, TProperty> Range(int min, int max, string message = null)
    {
        if (!_shouldValidate) return this;

        _validActions.Add(value =>
        {
            if (value is int v && (v > max || v < min))
                _parent.AddError(new ValidError(message ?? $"{_propertyName} must be between {min} and {max} ."));
        });

        return this;
    }

    public RuleBuilder<TModel, TProperty> Max(int max, string message = null)
    {
        if (!_shouldValidate) return this;

        _validActions.Add(value =>
        {
            if (value is int v && v > max)
                _parent.AddError(new ValidError(message ?? $"{_propertyName} must be greater {max} ."));
        });

        return this;
    }

    public RuleBuilder<TModel, TProperty> Min(int min, string message = null)
    {
        if (!_shouldValidate) return this;

        _validActions.Add(value =>
        {
            if (value is int v && v < min)
                _parent.AddError(new ValidError(message ?? $"{_propertyName} must be less {min} ."));
        });


        return this;
    }

    public RuleBuilder<TModel, TProperty> Length(int min, int max, string message = null)
    {
        if (!_shouldValidate) return this;
        _validActions.Add(value =>
        {
            if (value is string str && (str.Length > max || str.Length < min))
                _parent.AddError(new ValidError(message ?? $"{_propertyName} length must be between {min} and {max} characters."));
        });


        return this;
    }

    public RuleBuilder<TModel, TProperty> MaxLength(int max, string message = null)
    {
        if (!_shouldValidate) return this;

        _validActions.Add(value =>
        {
            if (value is string str && str.Length > max)
                _parent.AddError(new ValidError(message ?? $"{_propertyName} length must be greater {max} characters."));
        });

        return this;
    }

    public RuleBuilder<TModel, TProperty> MinLength(int min, string message = null)
    {
        if (!_shouldValidate) return this;

        _validActions.Add(value =>
        {
            if (value is string str && str.Length < min)
                _parent.AddError(new ValidError(message ?? $"{_propertyName} length must be less {min} characters."));
        });

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsEmail(string message = null)
    {
        if (!_shouldValidate) return this;

        _validActions.Add(value =>
        {
            if (value is string str && !str.Contains('@'))
                _parent.AddError(new ValidError(message ?? $"{_propertyName} is not a valid email"));
        });

        return this;
    }

    public RuleBuilder<TModel, TProperty> Custom(Func<TProperty, bool> func, string message = null)
    {
        if (!_shouldValidate) return this;

        _validActions.Add(value =>
        {
            if (!func(value))
                _parent.AddError(new ValidError(message ?? $"{_propertyName} is not valid."));
        });

        return this;
    }

    public RuleBuilder<TModel, TProperty> Custom(Func<bool> func, string message = null)
    {
        if (!_shouldValidate) return this;

        _validActions.Add(value =>
        {
            if (!func.Invoke())
                _parent.AddError(new ValidError(message ?? "Error valid."));
        });

        return this;
    }
}
