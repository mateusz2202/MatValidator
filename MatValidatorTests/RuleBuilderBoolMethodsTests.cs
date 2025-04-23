using MatValidator;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace MatValidatorTests;
public class RuleBuilderBoolMethodsTests
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

    public RuleBuilderBoolMethodsTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void IsTrue_ShouldValidateBooleanTrue()
    {
        // Arrange
        var model = new TestModel { IsActive = false };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.IsActive)
            .IsTrue("IsActive must be true.");

        // Act
        var result = validator.Validate(model);
        _output.WriteLine(JsonSerializer.Serialize(result));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages);
        Assert.Equal("IsActive must be true.", result.ErrorMessages[0]);
    }

    [Fact]
    public void IsFalse_ShouldValidateBooleanFalse()
    {
        // Arrange
        var model = new TestModel { IsDeleted = true };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.IsDeleted)
            .IsFalse("IsDeleted must be false.");

        // Act
        var result = validator.Validate(model);
        _output.WriteLine(JsonSerializer.Serialize(result));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages);
        Assert.Equal("IsDeleted must be false.", result.ErrorMessages[0]);
    }
}
