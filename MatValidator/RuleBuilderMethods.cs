using System.Text.RegularExpressions;

namespace MatValidator;
public partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
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
    public RuleBuilder<TModel, TProperty> Must(Func<TProperty, bool> func, string message = null)
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
                    _parent.AddError(new ValidError($"{_propertyName}.{error}"));

                return null;
            }
        ));

        return this;
    }

    public RuleBuilder<TModel, TProperty> Matches(string pattern, string message = null)
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

    public RuleBuilder<TModel, TProperty> Equal(TProperty expected, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (!Equals(value, expected))
                    return new ValidError(message ?? $"{_propertyName} must be equal to {expected}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> NotEqual(TProperty unexpected, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (Equals(value, unexpected))
                    return new ValidError(message ?? $"{_propertyName} must not be equal to {unexpected}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsIn(IEnumerable<TProperty> list, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (!list.Contains(value))
                    return new ValidError(message ?? $"{_propertyName} must be one of: {string.Join(", ", list)}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> NotIn(IEnumerable<TProperty> list, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (list.Contains(value))
                    return new ValidError(message ?? $"{_propertyName} must not be one of: {string.Join(", ", list)}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }
    public RuleBuilder<TModel, TProperty> GreaterThan(TProperty threshold, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is IComparable comparable && comparable.CompareTo(threshold) <= 0)
                    return new ValidError(message ?? $"{_propertyName} must be greater than {threshold}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> LessThan(TProperty threshold, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is IComparable comparable && comparable.CompareTo(threshold) >= 0)
                    return new ValidError(message ?? $"{_propertyName} must be less than {threshold}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> GreaterThanOrEqual(TProperty threshold, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is IComparable comparable && comparable.CompareTo(threshold) < 0)
                    return new ValidError(message ?? $"{_propertyName} must be greater than or equal to {threshold}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> LessThanOrEqual(TProperty threshold, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is IComparable comparable && comparable.CompareTo(threshold) > 0)
                    return new ValidError(message ?? $"{_propertyName} must be less than or equal to {threshold}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }
    public RuleBuilder<TModel, TProperty> IsEmpty(string message = null)
    {
        _validators.Add((
         _nextCondition,
         value =>
         {
             var isEmpty = value switch
             {
                 null => true,
                 string str => string.IsNullOrWhiteSpace(str),
                 System.Collections.IEnumerable enumerable => !enumerable.Cast<object>().Any(),
                 _ => false
             };

             if (!isEmpty)
                 return new ValidError(message ?? $"{_propertyName} must be empty");

             return null;
         }
        ));

        _nextCondition = _ => true;

        return this;
    }


    public RuleBuilder<TModel, TProperty> NotEmpty(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                var isEmpty = value switch
                {
                    null => true,
                    string str => string.IsNullOrWhiteSpace(str),
                    System.Collections.IEnumerable enumerable => !enumerable.Cast<object>().Any(),
                    _ => false
                };

                if (isEmpty)
                    return new ValidError(message ?? $"{_propertyName} cannot be empty");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsNull(string message = null)
    {
        _validators.Add((
            _nextCondition,
             value => value is not null
                    ? new ValidError(message ?? $"{_propertyName} must be null")
                    : null
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> NotNull(string message = null)
    {
        _validators.Add((
            _nextCondition,
             value => value is null
                    ? new ValidError(message ?? $"{_propertyName} cannot be null")
                    : null
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

    public RuleBuilder<TModel, TProperty> IsUrl(string message = null)
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

    public RuleBuilder<TModel, TProperty> IsAlpha(string message = null)
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

    public RuleBuilder<TModel, TProperty> IsAlphanumeric(string message = null)
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

    public RuleBuilder<TModel, TProperty> StartsWith(string prefix, string message = null)
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

    public RuleBuilder<TModel, TProperty> EndsWith(string suffix, string message = null)
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

    public RuleBuilder<TModel, TProperty> IsTrue(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is bool b && !b)
                    return new ValidError(message ?? $"{_propertyName} must be true.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsFalse(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is bool b && b)
                    return new ValidError(message ?? $"{_propertyName} must be false.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> OneOf(params IEnumerable<TProperty> options)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (!options.Contains(value))
                    return new ValidError($"{_propertyName} must be one of the following values: {string.Join(", ", options)}");

                return null;
            }
        ));

        _nextCondition = _ => true;
        return this;
    }

    public RuleBuilder<TModel, TProperty> NoneOf(params IEnumerable<TProperty> options)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (options.Contains(value))
                    return new ValidError($"{_propertyName} must not be one of the following values: {string.Join(", ", options)}");

                return null;
            }
        ));

        _nextCondition = _ => true;
        return this;
    }

    public RuleBuilder<TModel, TProperty> IsInEnum(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value == null || !Enum.IsDefined(typeof(TProperty), value))
                    return new ValidError(message ?? $"{_propertyName} must be a valid enum value.");

                return null;
            }
        ));

        _nextCondition = _ => true;
        return this;
    }

    public RuleBuilder<TModel, TProperty> IsInThePast(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt >= DateTimeOffset.Now)
                    return new ValidError(message ?? $"{_propertyName} must be in the past.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsInTheFuture(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt <= DateTimeOffset.Now)
                    return new ValidError(message ?? $"{_propertyName} must be in the future.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsBefore(DateTimeOffset date, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt >= date)
                    return new ValidError(message ?? $"{_propertyName} must be before {date}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsAfter(DateTimeOffset date, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt <= date)
                    return new ValidError(message ?? $"{_propertyName} must be after {date}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsBetween(DateTimeOffset start, DateTime end, string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && (dt < start || dt > end))
                    return new ValidError(message ?? $"{_propertyName} must be between {start} and {end}.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsOnWeekend(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && !(dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday))
                    return new ValidError(message ?? $"{_propertyName} must be on a weekend.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsToday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt.Date != DateTimeOffset.Now.Date)
                    return new ValidError(message ?? $"{_propertyName} must be today's date.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsMonday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt.DayOfWeek != DayOfWeek.Monday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Monday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsTuesday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt.DayOfWeek != DayOfWeek.Tuesday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Tuesday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsWednesday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt.DayOfWeek != DayOfWeek.Wednesday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Wednesday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsThursday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt.DayOfWeek != DayOfWeek.Thursday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Thursday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsFriday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt.DayOfWeek != DayOfWeek.Friday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Friday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsSaturday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt.DayOfWeek != DayOfWeek.Saturday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Saturday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

    public RuleBuilder<TModel, TProperty> IsSunday(string message = null)
    {
        _validators.Add((
            _nextCondition,
            value =>
            {
                if (value is DateTimeOffset dt && dt.DayOfWeek != DayOfWeek.Sunday)
                    return new ValidError(message ?? $"{_propertyName} must be on a Sunday.");

                return null;
            }
        ));

        _nextCondition = _ => true;

        return this;
    }

}
