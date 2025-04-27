using System.Text.RegularExpressions;

namespace MatValidator;
public sealed partial class RuleBuilder<TModel, TProperty> : IValidatorRule
{
    internal RuleBuilder<TModel, TProperty> Length(int min, int max, string message = null)
        => AddValidator(new LengthValidator<TModel, TProperty>(_propertyName, min, max, message));

    internal RuleBuilder<TModel, TProperty> MaxLength(int max, string message = null)
        => AddValidator(new MaxLengthValidator<TModel, TProperty>(_propertyName, max, message));

    internal RuleBuilder<TModel, TProperty> MinLength(int min, string message = null)
        => AddValidator(new MinLengthValidator<TModel, TProperty>(_propertyName, min, message));

    internal RuleBuilder<TModel, TProperty> IsEmail(string message = null)
        => AddValidator(new IsEmailValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsUrl(string message = null)
        => AddValidator(new IsUrlValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsAlpha(string message = null)
        => AddValidator(new IsAlphaValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsAlphanumeric(string message = null)
        => AddValidator(new IsAlphanumericValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> StartsWith(string prefix, string message = null)
        => AddValidator(new StartsWithValidator<TModel, TProperty>(_propertyName, prefix, message));

    internal RuleBuilder<TModel, TProperty> EndsWith(string suffix, string message = null)
        => AddValidator(new EndsWithValidator<TModel, TProperty>(_propertyName, suffix, message));

    internal RuleBuilder<TModel, TProperty> Matches(string pattern, string message = null)
        => AddValidator(new MatchesValidator<TModel, TProperty>(_propertyName, pattern, message));
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

internal sealed class LengthValidator<TModel, TProperty>(string propertyName, int min, int max, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    private readonly int _min = min;
    private readonly int _max = max;

    public string? Validate<T>(T value)
        => (value is string str && (str.Length > _max || str.Length < _min)) ? _message ?? $"{_propertyName} length must be between {_min} and {_max} characters." : null;
}

internal sealed class MaxLengthValidator<TModel, TProperty>(string propertyName, int max, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    private readonly int _max = max;

    public string? Validate<T>(T value)
        => (value is string str && str.Length > _max) ? _message ?? $"{_propertyName} length must be less than {_max} characters." : null;
}

internal sealed class MinLengthValidator<TModel, TProperty>(string propertyName, int min, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    private readonly int _min = min;

    public string? Validate<T>(T value)
        => (value is string str && str.Length < _min) ? _message ?? $"{_propertyName} length must be greater than {_min} characters." : null;
}

internal sealed class IsEmailValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{

    public string? Validate<T>(T value)
        => (value is string str && !str.Contains('@')) ? _message ?? $"{_propertyName} is not a valid email" : null;
}

internal sealed class IsUrlValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{

    public string? Validate<T>(T value)
        => (value is string str && !Uri.IsWellFormedUriString(str, UriKind.Absolute)) ? _message ?? $"{_propertyName} is not a valid URL." : null;
}

internal sealed class IsAlphaValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is string str && !str.All(char.IsLetter)) ? _message ?? $"{_propertyName} must contain only letters." : null;
}

internal sealed class IsAlphanumericValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is string str && !str.All(char.IsLetterOrDigit)) ? _message ?? $"{_propertyName} must be alphanumeric." : null;
}

internal sealed class StartsWithValidator<TModel, TProperty>(string propertyName, string prefix, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    private readonly string _prefix = prefix;
    public string? Validate<T>(T value)
        => (value is string str && !str.StartsWith(_prefix)) ? _message ?? $"{_propertyName} must start with '{_prefix}'." : null;
}

internal sealed class EndsWithValidator<TModel, TProperty>(string propertyName, string suffix, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    private readonly string _suffix = suffix;
    public string? Validate<T>(T value)
        => (value is string str && !str.EndsWith(_suffix)) ? _message ?? $"{_propertyName} must end with '{_suffix}'." : null;
}

internal sealed class MatchesValidator<TModel, TProperty>(string propertyName, string pattern, string? message)
     : BaseValidator(propertyName, message), IValidatorProperty
{
    private readonly string _pattern = pattern;
    public string? Validate<T>(T value)
        => (value is string str && !Regex.IsMatch(str, _pattern)) ? _message ?? $"{_propertyName} is not in the correct format." : null;
}