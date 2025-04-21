using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MatValidator;

public record ValidResult(bool IsValid, List<string> ErrorMessages);
public record ValidError(bool IsValid)
{
    public string ErrorMessage { get; set; } = string.Empty;
    public ValidError SetMessage(string message)
    {
        if (!IsValid)
            ErrorMessage = message;

        return this;
    }
}
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
    private readonly Validator _validator;

    public ValidatorBuilder()
    {
        _validErrors = [];
        _validator = new Validator(HandleResult);
    }

    public Validator AddRule()
    {
        return _validator;
    }

    public ValidResult Validate()
    {
        var isValid = _validErrors.All(x => x.IsValid);
        var errorMessages = _validErrors.Where(x => !x.IsValid).Select(x => x.ErrorMessage).ToList();
        return new ValidResult(isValid, errorMessages);
    }


    private void HandleResult(ValidError result)
    {
        _validErrors.Add(result);
    }
}


public class Validator
{
    private readonly Action<ValidError> _notifyBuilder;

    public Validator(Action<ValidError> notifyBuilder)
    {
        _notifyBuilder = notifyBuilder;
    }

    public ValidError NotEmpty(Property<string> property, string message = null)
    {
        var result = new ValidError(!string.IsNullOrWhiteSpace(property.Value))
            .SetMessage(message ?? $"{property.Name} is required");

        _notifyBuilder.Invoke(result);

        return result;
    }

    public ValidError NotEmpty<T>(Property<T> property, string message = null)
    {
        var result = new ValidError(property.Value is not null)
            .SetMessage(message ?? $"{property.Name} is required");

        _notifyBuilder.Invoke(result);

        return result;
    }

    public ValidError Range(Property<int> property, int min, int max, string message = null)
    {
        var result = new ValidError(true);

        if (property.Value > max || property.Value < min)
        {
            result = new ValidError(false).SetMessage(message ?? $"{property.Name} must be between {min} and {max} .");
        }

        _notifyBuilder.Invoke(result);

        return result;
    }

    public ValidError Max(Property<int> property, int max, string message = null)
    {
        var result = new ValidError(true);

        if (property.Value > max)
        {
            result = new ValidError(false).SetMessage(message ?? $"{property.Name} must be greater {max} .");
        }

        _notifyBuilder.Invoke(result);

        return result;
    }

    public ValidError Min(Property<int> property, int min, string message = null)
    {
        var result = new ValidError(true);

        if (property.Value < min)
        {
            result = new ValidError(false).SetMessage(message ?? $"{property.Name} must be less {min} .");
        }

        _notifyBuilder.Invoke(result);

        return result;
    }

    public ValidError Length(Property<string> property, int min, int max, string message = null)
    {
        var result = new ValidError(true);

        if (property.Value.Length > max || property.Value.Length < min)
        {
            result = new ValidError(false).SetMessage(message ?? $"{property.Name} lenght must be between {min} and {max} characters.");
        }

        _notifyBuilder.Invoke(result);

        return result;
    }

    public ValidError MaxLength(Property<string> property, int max, string message = null)
    {
        var result = new ValidError(true);

        if (property.Value.Length > max)
        {
            result = new ValidError(false).SetMessage(message ?? $"{property.Name} lenght must be greater {max} characters.");
        }

        _notifyBuilder.Invoke(result);

        return result;
    }

    public ValidError MinLength(Property<string> property, int min, string message = null)
    {
        var result = new ValidError(true);

        if (property.Value.Length < min)
        {
            result = new ValidError(false).SetMessage(message ?? $"{property.Name} lenght must be less {min} characters.");
        }

        _notifyBuilder.Invoke(result);

        return result;
    }

    public ValidError IsEmail(Property<string> property, string message = null)
    {
        var result = new ValidError(true);

        if (!property.Value.Contains('@'))
        {
            result = new ValidError(false).SetMessage(message ?? $"{property.Name} is not a valid email.");
        }

        _notifyBuilder.Invoke(result);

        return result;
    }

    public ValidError Custom<T>(Property<T> property, Func<T, bool> func, string message = null)
    {
        var result = new ValidError(true);

        if (!func(property.Value))
        {
            result = new ValidError(false).SetMessage(message ?? $"{property.Name} is not valid.");
        }

        _notifyBuilder.Invoke(result);

        return result;
    }

    public ValidError Custom(Func<bool> func, string message = null)
    {
        var result = new ValidError(true);

        if (!func.Invoke())
        {
            result = new ValidError(false).SetMessage(message ?? "Error valid.");
        }

        _notifyBuilder.Invoke(result);

        return result;
    }
}
