using MatValidator;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace MatValidatorTests;

public class RuleBuilderDateMethodsTests
{
    private class Event
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public DateTime SpecialDay { get; set; }

        public Event(DateTime startDate, DateTime endDate, DateTime registrationDeadline, DateTime specialDay)
        {
            StartDate = startDate;
            EndDate = endDate;
            RegistrationDeadline = registrationDeadline;
            SpecialDay = specialDay;
        }
    }
    private readonly ITestOutputHelper _output;

    public RuleBuilderDateMethodsTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task IsInThePast_ShouldValidatePastDates()
    {
        // Arrange
        var futureDate = DateTime.Now.AddDays(1);
        var @event = new Event(futureDate, DateTime.Now, DateTime.Now, DateTime.Now);
        var validator = new ValidatorBuilder<Event>();

        validator
            .RuleFor(x => x.StartDate)
            .IsInThePast("Start date must be in the past.");

        // Act
        var result = await validator.ValidateAsync(@event, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Start date must be in the past.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task IsInTheFuture_ShouldValidateFutureDates()
    {
        // Arrange
        var pastDate = DateTime.Now.AddDays(-1);
        var @event = new Event(DateTime.Now, pastDate, DateTime.Now, DateTime.Now);
        var validator = new ValidatorBuilder<Event>();

        validator
            .RuleFor(x => x.EndDate)
            .IsInTheFuture("End date must be in the future.");

        // Act
        var result = await validator.ValidateAsync(@event, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("End date must be in the future.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task IsBefore_ShouldValidateDatesBeforeReference()
    {
        // Arrange
        var referenceDate = DateTime.Now.AddDays(5);
        var @event = new Event(referenceDate.AddDays(1), DateTime.Now, DateTime.Now, DateTime.Now);
        var validator = new ValidatorBuilder<Event>();

        validator
            .RuleFor(x => x.StartDate)
            .IsBefore(referenceDate, "Start date must be before reference date.");

        // Act
        var result = await validator.ValidateAsync(@event, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Start date must be before reference date.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task IsAfter_ShouldValidateDatesAfterReference()
    {
        // Arrange
        var referenceDate = DateTime.Now.AddDays(-5);
        var @event = new Event(DateTime.Now, referenceDate.AddDays(-1), DateTime.Now, DateTime.Now);
        var validator = new ValidatorBuilder<Event>();

        validator
            .RuleFor(x => x.EndDate)
            .IsAfter(referenceDate, "End date must be after reference date.");

        // Act
        var result = await validator.ValidateAsync(@event, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("End date must be after reference date.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task IsBetween_ShouldValidateDatesInRange()
    {
        // Arrange
        var startRange = DateTime.Now.AddDays(-10);
        var endRange = DateTime.Now.AddDays(10);
        var @event = new Event(DateTime.Now, DateTime.Now, DateTime.Now.AddDays(-11), DateTime.Now);
        var validator = new ValidatorBuilder<Event>();

        validator
            .RuleFor(x => x.RegistrationDeadline)
            .IsBetween(startRange, endRange, "Registration deadline must be within the specified range.");

        // Act
        var result = await validator.ValidateAsync(@event, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Registration deadline must be within the specified range.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task IsOnWeekend_ShouldValidateWeekendDates()
    {
        // Arrange - Create a known weekday date (not Saturday or Sunday)
        var weekdayDate = new DateTime(2023, 10, 4); // Wednesday
        var @event = new Event(DateTime.Now, DateTime.Now, DateTime.Now, weekdayDate);
        var validator = new ValidatorBuilder<Event>();

        validator
            .RuleFor(x => x.SpecialDay)
            .IsOnWeekend("Special day must be on a weekend.");

        // Act
        var result = await validator.ValidateAsync(@event, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Special day must be on a weekend.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task IsToday_ShouldValidateCurrentDate()
    {
        // Arrange
        var notToday = DateTime.Now.AddDays(1);
        var @event = new Event(DateTime.Now, DateTime.Now, DateTime.Now, notToday);
        var validator = new ValidatorBuilder<Event>();

        validator
            .RuleFor(x => x.SpecialDay)
            .IsToday("Special day must be today.");

        // Act
        var result = await validator.ValidateAsync(@event, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Special day must be today.", result.ErrorMessages.ToArray()[0]);
    }

    [Theory]
    [InlineData(DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Friday)]
    [InlineData(DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Sunday)]
    public async Task SpecificDayOfWeek_ShouldValidateCorrectDay(DayOfWeek dayOfWeek)
    {
        // Arrange
        var testDate = GetNextWeekday(((int)dayOfWeek + 1) == 7 ? 0 : dayOfWeek + 1);
        var @event = new Event(DateTime.Now, DateTime.Now, DateTime.Now, testDate);
        var validator = new ValidatorBuilder<Event>();

        switch (dayOfWeek)
        {
            case DayOfWeek.Monday:
                validator.RuleFor(x => x.SpecialDay).IsMonday("Must be Monday.");
                break;
            case DayOfWeek.Tuesday:
                validator.RuleFor(x => x.SpecialDay).IsTuesday("Must be Tuesday.");
                break;
            case DayOfWeek.Wednesday:
                validator.RuleFor(x => x.SpecialDay).IsWednesday("Must be Wednesday.");
                break;
            case DayOfWeek.Thursday:
                validator.RuleFor(x => x.SpecialDay).IsThursday("Must be Thursday.");
                break;
            case DayOfWeek.Friday:
                validator.RuleFor(x => x.SpecialDay).IsFriday("Must be Friday.");
                break;
            case DayOfWeek.Saturday:
                validator.RuleFor(x => x.SpecialDay).IsSaturday("Must be Saturday.");
                break;
            case DayOfWeek.Sunday:
                validator.RuleFor(x => x.SpecialDay).IsSunday("Must be Sunday.");
                break;
        }

        // Act
        var result = await validator.ValidateAsync(@event, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal($"Must be {dayOfWeek}.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task CombinedDateValidations_ShouldWorkTogether()
    {
        // Arrange
        var @event = new Event(
            startDate: DateTime.Now.AddDays(1), // Should be in past
            endDate: DateTime.Now.AddDays(-1), // Should be in future
            registrationDeadline: DateTime.Now.AddDays(11), // Should be between -10 and +10 days
            specialDay: DateTime.Now.AddDays(1) // Should be today
        );

        var validator = new ValidatorBuilder<Event>();

        validator
            .RuleFor(x => x.StartDate)
            .IsInThePast("Start date must be in the past.");

        validator
            .RuleFor(x => x.EndDate)
            .IsInTheFuture("End date must be in the future.");

        validator
            .RuleFor(x => x.RegistrationDeadline)
            .IsBetween(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(10),
                "Registration deadline must be within 10 days range.");

        validator
            .RuleFor(x => x.SpecialDay)
            .IsToday("Special day must be today.");

        // Act
        var result = await validator.ValidateAsync(@event, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(4, result.ErrorMessages.Count);
    }

    private DateTime GetNextWeekday(DayOfWeek day)
    {
        DateTime result = DateTime.Now.AddDays(1);
        for (int i = 0; i < 7; i++)
        {
            if (result.DayOfWeek == day)
                return result;
            result = result.AddDays(1);
        }
        throw new InvalidOperationException("Could not find the requested weekday within 7 days.");
    }
}
