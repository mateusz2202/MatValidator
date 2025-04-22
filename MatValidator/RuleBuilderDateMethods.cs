namespace MatValidator;
public partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    internal RuleBuilder<TModel, TProperty> IsInThePast(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt >= DateTime.Now)
                    return new ValidError(message ?? $"{_propertyName} must be in the past.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsInTheFuture(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt <= DateTime.Now)
                    return new ValidError(message ?? $"{_propertyName} must be in the future.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsBefore(DateTime date, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt >= date)
                    return new ValidError(message ?? $"{_propertyName} must be before {date}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsAfter(DateTime date, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt <= date)
                    return new ValidError(message ?? $"{_propertyName} must be after {date}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsBetween(DateTime start, DateTime end, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && (dt < start || dt > end))
                    return new ValidError(message ?? $"{_propertyName} must be between {start} and {end}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsOnWeekend(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && !(dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday))
                    return new ValidError(message ?? $"{_propertyName} must be on a weekend.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsToday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt.Date != DateTime.Now.Date)
                    return new ValidError(message ?? $"{_propertyName} must be today's date.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsMonday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Monday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Monday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsTuesday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Tuesday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Tuesday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsWednesday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Wednesday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Wednesday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsThursday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Thursday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Thursday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsFriday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Friday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Friday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsSaturday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Saturday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Saturday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    internal RuleBuilder<TModel, TProperty> IsSunday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTime dt && dt.DayOfWeek != DayOfWeek.Sunday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Sunday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }
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
}
