namespace MatValidator;
public sealed partial class RuleBuilder<TModel, TProperty> : IValidatorRule
{
    internal RuleBuilder<TModel, TProperty> IsInThePast(string message = null)
        => AddValidator(new IsInThePastValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsInTheFuture(string message = null)
        => AddValidator(new IsInTheFutureValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsBefore(DateTime date, string message = null)
        => AddValidator(new IsBeforeValidator<TModel, TProperty>(_propertyName, date, message));

    internal RuleBuilder<TModel, TProperty> IsAfter(DateTime date, string message = null)
        => AddValidator(new IsAfterValidator<TModel, TProperty>(_propertyName, date, message));

    internal RuleBuilder<TModel, TProperty> IsBetween(DateTime start, DateTime end, string message = null)
        => AddValidator(new IsBetweenValidator<TModel, TProperty>(_propertyName, start, end, message));

    internal RuleBuilder<TModel, TProperty> IsOnWeekend(string message = null)
        => AddValidator(new IsOnWeekendValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsToday(string message = null)
        => AddValidator(new IsTodayValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsMonday(string message = null)
        => AddValidator(new IsMondayValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsTuesday(string message = null)
        => AddValidator(new IsTuesdayValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsWednesday(string message = null)
        => AddValidator(new IsWednesdayValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsThursday(string message = null)
        => AddValidator(new IsThursdayValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsFriday(string message = null)
        => AddValidator(new IsFridayValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsSaturday(string message = null)
        => AddValidator(new IsSaturdayValidator<TModel, TProperty>(_propertyName, message));

    internal RuleBuilder<TModel, TProperty> IsSunday(string message = null)
        => AddValidator(new IsSundayValidator<TModel, TProperty>(_propertyName, message));
}

public static class DateTimeRuleBuilderExtensions
{
    public static RuleBuilder<TModel, DateTime> IsInThePast<TModel>(this RuleBuilder<TModel, DateTime> builder, string message = null)
        => builder.IsInThePast(message);

    public static RuleBuilder<TModel, DateTime> IsInTheFuture<TModel>(this RuleBuilder<TModel, DateTime> builder, string message = null)
        => builder.IsInTheFuture(message);

    public static RuleBuilder<TModel, DateTime> IsBefore<TModel>(this RuleBuilder<TModel, DateTime> builder, DateTime date, string message = null)
        => builder.IsBefore(date, message);

    public static RuleBuilder<TModel, DateTime> IsAfter<TModel>(this RuleBuilder<TModel, DateTime> builder, DateTime date, string message = null)
        => builder.IsAfter(date, message);

    public static RuleBuilder<TModel, DateTime> IsBetween<TModel>(this RuleBuilder<TModel, DateTime> builder, DateTime start, DateTime end, string message = null)
        => builder.IsBetween(start, end, message);

    public static RuleBuilder<TModel, DateTime> IsOnWeekend<TModel>(this RuleBuilder<TModel, DateTime> builder, string message = null)
        => builder.IsOnWeekend(message);

    public static RuleBuilder<TModel, DateTime> IsToday<TModel>(this RuleBuilder<TModel, DateTime> builder, string message = null)
        => builder.IsToday(message);

    public static RuleBuilder<TModel, DateTime> IsMonday<TModel>(this RuleBuilder<TModel, DateTime> builder, string message = null)
        => builder.IsMonday(message);

    public static RuleBuilder<TModel, DateTime> IsTuesday<TModel>(this RuleBuilder<TModel, DateTime> builder, string message = null)
        => builder.IsTuesday(message);

    public static RuleBuilder<TModel, DateTime> IsWednesday<TModel>(this RuleBuilder<TModel, DateTime> builder, string message = null)
        => builder.IsWednesday(message);

    public static RuleBuilder<TModel, DateTime> IsThursday<TModel>(this RuleBuilder<TModel, DateTime> builder, string message = null)
        => builder.IsThursday(message);

    public static RuleBuilder<TModel, DateTime> IsFriday<TModel>(this RuleBuilder<TModel, DateTime> builder, string message = null)
        => builder.IsFriday(message);

    public static RuleBuilder<TModel, DateTime> IsSaturday<TModel>(this RuleBuilder<TModel, DateTime> builder, string message = null)
        => builder.IsSaturday(message);

    public static RuleBuilder<TModel, DateTime> IsSunday<TModel>(this RuleBuilder<TModel, DateTime> builder, string message = null)
        => builder.IsSunday(message);

    public static RuleBuilder<TModel, DateTime?> IsInThePast<TModel>(this RuleBuilder<TModel, DateTime?> builder, string message = null)
    => builder.IsInThePast(message);

    public static RuleBuilder<TModel, DateTime?> IsInTheFuture<TModel>(this RuleBuilder<TModel, DateTime?> builder, string message = null)
        => builder.IsInTheFuture(message);

    public static RuleBuilder<TModel, DateTime?> IsBefore<TModel>(this RuleBuilder<TModel, DateTime?> builder, DateTime? date, string message = null)
        => builder.IsBefore(date, message);

    public static RuleBuilder<TModel, DateTime?> IsAfter<TModel>(this RuleBuilder<TModel, DateTime?> builder, DateTime? date, string message = null)
        => builder.IsAfter(date, message);

    public static RuleBuilder<TModel, DateTime?> IsBetween<TModel>(this RuleBuilder<TModel, DateTime?> builder, DateTime? start, DateTime? end, string message = null)
        => builder.IsBetween(start, end, message);

    public static RuleBuilder<TModel, DateTime?> IsOnWeekend<TModel>(this RuleBuilder<TModel, DateTime?> builder, string message = null)
        => builder.IsOnWeekend(message);

    public static RuleBuilder<TModel, DateTime?> IsToday<TModel>(this RuleBuilder<TModel, DateTime?> builder, string message = null)
        => builder.IsToday(message);

    public static RuleBuilder<TModel, DateTime?> IsMonday<TModel>(this RuleBuilder<TModel, DateTime?> builder, string message = null)
        => builder.IsMonday(message);

    public static RuleBuilder<TModel, DateTime?> IsTuesday<TModel>(this RuleBuilder<TModel, DateTime?> builder, string message = null)
        => builder.IsTuesday(message);

    public static RuleBuilder<TModel, DateTime?> IsWednesday<TModel>(this RuleBuilder<TModel, DateTime?> builder, string message = null)
        => builder.IsWednesday(message);

    public static RuleBuilder<TModel, DateTime?> IsThursday<TModel>(this RuleBuilder<TModel, DateTime?> builder, string message = null)
        => builder.IsThursday(message);

    public static RuleBuilder<TModel, DateTime?> IsFriday<TModel>(this RuleBuilder<TModel, DateTime?> builder, string message = null)
        => builder.IsFriday(message);

    public static RuleBuilder<TModel, DateTime?> IsSaturday<TModel>(this RuleBuilder<TModel, DateTime?> builder, string message = null)
        => builder.IsSaturday(message);

    public static RuleBuilder<TModel, DateTime?> IsSunday<TModel>(this RuleBuilder<TModel, DateTime?> builder, string message = null)
        => builder.IsSunday(message);
}

internal sealed class IsInThePastValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is DateTime dt && dt >= DateTime.Now) ? _message ?? $"{_propertyName} must be in the past." : null;
}

internal sealed class IsInTheFutureValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
       => (value is DateTime dt && dt <= DateTime.Now) ? _message ?? $"{_propertyName} must be in the future." : null;
}

internal sealed class IsBeforeValidator<TModel, TProperty>(string propertyName, DateTime? date, string? message)
        : BaseValidator(propertyName, message), IValidatorProperty
{
    private readonly DateTime? _date = date;
    public string? Validate<T>(T value)
        => (value is DateTime dt && dt >= _date) ? _message ?? $"{_propertyName} must be before {_date}." : null;
}

internal sealed class IsAfterValidator<TModel, TProperty>(string propertyName, DateTime? date, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    private readonly DateTime? _date = date;
    public string? Validate<T>(T value)
        => (value is DateTime dt && dt <= _date) ? _message ?? $"{_propertyName} must be after {_date}." : null;
}
internal sealed class IsBetweenValidator<TModel, TProperty>(string propertyName, DateTime? start, DateTime? end, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    private readonly DateTime? _start = start;
    private readonly DateTime? _end = end;

    public string? Validate<T>(T value)
        => (value is DateTime dt && (dt < _start || dt > _end)) ? _message ?? $"{_propertyName} must be between {_start} and {_end}." : null;
}

internal sealed class IsOnWeekendValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is DateTime dt && !(dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)) ? _message ?? $"{_propertyName} must be on a weekend." : null;
}

internal sealed class IsTodayValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is DateTime dt && dt.Date != DateTime.Now.Date) ? _message ?? $"{_propertyName} must be today's date." : null;
}

internal sealed class IsMondayValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Monday) ? _message ?? $"{_propertyName} must be on a Monday." : null;
}

internal sealed class IsTuesdayValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Tuesday) ? _message ?? $"{_propertyName} must be on a Tuesday." : null;
}

internal sealed class IsWednesdayValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Wednesday) ? _message ?? $"{_propertyName} must be on a Wednesday." : null;
}

internal sealed class IsThursdayValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Thursday) ? _message ?? $"{_propertyName} must be on a Thursday." : null;
}

internal sealed class IsFridayValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Friday) ? _message ?? $"{_propertyName} must be on a Friday." : null;
}

internal sealed class IsSaturdayValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Saturday) ? _message ?? $"{_propertyName} must be on a Saturday." : null;
}

internal sealed class IsSundayValidator<TModel, TProperty>(string propertyName, string? message)
    : BaseValidator(propertyName, message), IValidatorProperty
{
    public string? Validate<T>(T value)
        => (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Sunday) ? _message ?? $"{_propertyName} must be on a Sunday." : null;
}