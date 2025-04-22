using System.Text.RegularExpressions;

namespace MatValidator;
public partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    internal RuleBuilder<TModel, TProperty> Length(int min, int max, string message = null)
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

    internal RuleBuilder<TModel, TProperty> MaxLength(int max, string message = null)
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

    internal RuleBuilder<TModel, TProperty> MinLength(int min, string message = null)
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

    internal RuleBuilder<TModel, TProperty> IsEmail(string message = null)
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

    internal RuleBuilder<TModel, TProperty> IsUrl(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is string str && !Uri.IsWellFormedUriString(str, UriKind.Absolute))
                    return new ValidError(message ?? $"{_propertyName} is not a valid URL.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsAlpha(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is string str && !str.All(char.IsLetter))
                    return new ValidError(message ?? $"{_propertyName} must contain only letters.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsAlphanumeric(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is string str && !str.All(char.IsLetterOrDigit))
                    return new ValidError(message ?? $"{_propertyName} must be alphanumeric.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> StartsWith(string prefix, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is string str && !str.StartsWith(prefix))
                    return new ValidError(message ?? $"{_propertyName} must start with '{prefix}'.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> EndsWith(string suffix, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is string str && !str.EndsWith(suffix))
                    return new ValidError(message ?? $"{_propertyName} must end with '{suffix}'.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> Matches(string pattern, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is string str && !Regex.IsMatch(str, pattern))
                    return new ValidError(message ?? $"{_propertyName} is not in the correct format.");

                return null;
            }
        ));

        return this;
    }
}

public static class StringRuleBuilderExtensions
{
    public static RuleBuilder<TModel, string> Length<TModel>(this RuleBuilder<TModel, string> builder, int min, int max, string message = null)
        => builder.Length(min, max, message);

    public static RuleBuilder<TModel, string> MaxLength<TModel>(this RuleBuilder<TModel, string> builder, int max, string message = null)
        => builder.MaxLength(max, message);

    public static RuleBuilder<TModel, string> MinLength<TModel>(this RuleBuilder<TModel, string> builder, int min, string message = null)
        => builder.MinLength(min, message);

    public static RuleBuilder<TModel, string> IsEmail<TModel>(this RuleBuilder<TModel, string> builder, string message = null)
        => builder.IsEmail(message);

    public static RuleBuilder<TModel, string> IsUrl<TModel>(this RuleBuilder<TModel, string> builder, string message = null)
        => builder.IsUrl(message);

    public static RuleBuilder<TModel, string> IsAlpha<TModel>(this RuleBuilder<TModel, string> builder, string message = null)
        => builder.IsAlpha(message);

    public static RuleBuilder<TModel, string> IsAlphanumeric<TModel>(this RuleBuilder<TModel, string> builder, string message = null)
        => builder.IsAlphanumeric(message);

    public static RuleBuilder<TModel, string> StartsWith<TModel>(this RuleBuilder<TModel, string> builder, string prefix, string message = null)
        => builder.StartsWith(prefix, message);

    public static RuleBuilder<TModel, string> EndsWith<TModel>(this RuleBuilder<TModel, string> builder, string suffix, string message = null)
        => builder.EndsWith(suffix, message);

    public static RuleBuilder<TModel, string> Matches<TModel>(this RuleBuilder<TModel, string> builder, string pattern, string message = null)
        => builder.Matches(pattern, message);
}
