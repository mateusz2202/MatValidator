using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MatValidator;

public record ValidResult(bool IsValid, List<string> ErrorMessages);
public record ValidError(string ErrorMessage);

public record Property<T>(string Name, T Value);

public static class Property
{
    public static Property<T> Of<T>(T value, [CallerArgumentExpression(nameof(value))] string name = null)
         => new(name, value);

    public static Property<TProp> Of<TModel, TProp>(TModel instance, Expression<Func<TModel, TProp>> expr)
    {
        if (expr.Body is not MemberExpression memberExpr)
            throw new ArgumentException("Expression must be a property access", nameof(expr));

        var propertyName = memberExpr.Member.Name;
        var value = expr.Compile().Invoke(instance);
        return new Property<TProp>(propertyName, value);
    }
}
public class ValidatorBuilder
{
    private readonly List<ValidError> _validErrors;
    public ValidatorBuilder()
    {
        _validErrors = [];
    }

    public ValidResult Validate()
    {
        var isValid = _validErrors.Count == 0;
        var errorMessages = _validErrors.Select(x => x.ErrorMessage).ToList();
        return new ValidResult(isValid, errorMessages);
    }

    public RuleBuilder<TProp> RuleFor<TModel, TProp>(TModel model, Expression<Func<TModel, TProp>> expr)
    {
        var property = Property.Of(model, expr);
        return new RuleBuilder<TProp>(this, property);
    }

    internal void AddError(ValidError error)
    {
        _validErrors.Add(error);
    }
}

public class RuleBuilder<T>
{
    private readonly ValidatorBuilder _parent;
    private readonly Property<T> _property;
    private bool _shouldValidate = true;

    public RuleBuilder(ValidatorBuilder parent, Property<T> property)
    {
        _parent = parent;
        _property = property;
    }

    public RuleBuilder<T> When(Func<bool> condition)
    {
        _shouldValidate = condition();
        return this;
    }


    public RuleBuilder<T> NotEmpty(string message = null)
    {
        if (!_shouldValidate) return this;

        bool isValid = typeof(T) == typeof(string)
            ? !string.IsNullOrWhiteSpace(_property.Value as string)
            : _property.Value is not null;

        if (!isValid)
            _parent.AddError(new ValidError(message ?? $"{_property.Name} is required"));

        return this;
    }

    public RuleBuilder<T> Range(int min, int max, string message = null)
    {
        if (!_shouldValidate) return this;

        if (_property.Value is int v && (v > max || v < min))
            _parent.AddError(new ValidError(message ?? $"{_property.Name} must be between {min} and {max} ."));

        return this;
    }

    public RuleBuilder<T> Max(Property<int> property, int max, string message = null)
    {
        if (!_shouldValidate) return this;

        if (_property.Value is int v && v > max)
            _parent.AddError(new ValidError(message ?? $"{property.Name} must be greater {max} ."));

        return this;
    }

    public RuleBuilder<T> Min(int min, string message = null)
    {
        if (!_shouldValidate) return this;

        if (_property.Value is int v && v < min)
            _parent.AddError(new ValidError(message ?? $"{_property.Name} must be less {min} ."));

        return this;
    }

    public RuleBuilder<T> Length(int min, int max, string message = null)
    {
        if (!_shouldValidate) return this;

        if (_property.Value is string str && (str.Length > max || str.Length < min))
            _parent.AddError(new ValidError(message ?? $"{_property.Name} length must be between {min} and {max} characters."));

        return this;
    }

    public RuleBuilder<T> MaxLength(int max, string message = null)
    {
        if (!_shouldValidate) return this;

        if (_property.Value is string str && str.Length > max)
            _parent.AddError(new ValidError(message ?? $"{_property.Name} length must be greater {max} characters."));

        return this;
    }

    public RuleBuilder<T> MinLength(int min, string message = null)
    {
        if (!_shouldValidate) return this;

        if (_property.Value is string str && str.Length < min)
            _parent.AddError(new ValidError(message ?? $"{_property.Name} length must be less {min} characters."));

        return this;
    }

    public RuleBuilder<T> IsEmail(string message = null)
    {
        if (!_shouldValidate) return this;

        if (_property.Value is string str && !str.Contains('@'))
            _parent.AddError(new ValidError(message ?? $"{_property.Name} is not a valid email"));

        return this;
    }

    public RuleBuilder<T> Custom(Func<T, bool> func, string message = null)
    {
        if (!_shouldValidate) return this;

        if (!func(_property.Value))
            _parent.AddError(new ValidError(message ?? $"{_property.Name} is not valid."));

        return this;
    }

    public RuleBuilder<T> Custom(Func<bool> func, string message = null)
    {
        if (!_shouldValidate) return this;

        if (!func.Invoke())
            _parent.AddError(new ValidError(message ?? "Error valid."));

        return this;
    }
}
