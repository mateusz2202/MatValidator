using MatValidator;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace MatValidatorTests;
public class RuleBuilderComparableMethodsTests
{
    private class TestModel
    {
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Age { get; set; }
        public decimal Price { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Website { get; set; }
    }

    private readonly ITestOutputHelper _output;

    public RuleBuilderComparableMethodsTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task GreaterThan_ShouldValidateNumbers()
    {
        // Arrange
        var model = new TestModel { Age = 17 };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Age)
            .GreaterThan(18, "Age must be greater than 18.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Age must be greater than 18.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task LessThan_ShouldValidateNumbers()
    {
        // Arrange
        var model = new TestModel { Price = 100.01m };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Price)
            .LessThan(100, "Price must be less than 100.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Price must be less than 100.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task GreaterThanOrEqual_ShouldValidateNumbers()
    {
        // Arrange
        var model = new TestModel { Age = 17 };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Age)
            .GreaterThanOrEqual(18, "Age must be at least 18.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Age must be at least 18.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task LessThanOrEqual_ShouldValidateNumbers()
    {
        // Arrange
        var model = new TestModel { Price = 100.01m };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Price)
            .LessThanOrEqual(100, "Price must be at most 100.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Price must be at most 100.", result.ErrorMessages.ToArray()[0]);
    }
}
