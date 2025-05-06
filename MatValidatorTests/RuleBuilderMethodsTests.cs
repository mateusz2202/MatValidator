using MatValidator;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace MatValidatorTests;

public class RuleBuilderMethodsTests
{
    private class TestModel
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public List<string> Tags { get; set; }
        public string Status { get; set; }
        public UserRole Role { get; set; }
    }

    private enum UserRole
    {
        Guest,
        User,
        Admin
    }

    private readonly ITestOutputHelper _output;
    public RuleBuilderMethodsTests(ITestOutputHelper output)
    {
        _output = output;
    }
    [Fact]
    public async Task Must_ShouldValidateCustomCondition()
    {
        // Arrange
        var model = new TestModel { Name = "test" };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Name)
            .Must(name => name == "valid", "Name must be 'valid'.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Name must be 'valid'.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task Custom_ShouldValidateCustomFunction()
    {
        // Arrange
        var model = new TestModel();
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Name)
            .Custom(() => false, "Custom validation failed.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Custom validation failed.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task OverridePropertyName_ShouldChangePropertyNameInMessages()
    {
        // Arrange
        var model = new TestModel { Name = null };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Name)
            .OverridePropertyName("User Name")
            .NotNull("Field is required.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Field is required.", result.ErrorMessages.ToArray()[0]);
    }


    [Fact]
    public async Task Equal_ShouldValidateEquality()
    {
        // Arrange
        var model = new TestModel { Status = "pending" };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Status)
            .Equal("approved", "Status must be 'approved'.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Status must be 'approved'.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task NotEqual_ShouldValidateInequality()
    {
        // Arrange
        var model = new TestModel { Status = "invalid" };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Status)
            .NotEqual("invalid", "Status must not be 'invalid'.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Status must not be 'invalid'.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task IsEmpty_ShouldValidateEmptyValues()
    {
        // Arrange
        var model = new TestModel { Name = "not empty", Tags = new List<string> { "tag" } };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Name)
            .IsEmpty("Name must be empty.");

        validator.RuleFor(x => x.Tags)
            .IsEmpty("Tags must be empty.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.ErrorMessages.Count);
    }

    [Fact]
    public async Task NotEmpty_ShouldValidateNonEmptyValues()
    {
        // Arrange
        var model = new TestModel { Name = null, Tags = new List<string>() };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Name)
            .NotEmpty("Name is required.");

        validator.RuleFor(x => x.Tags)
            .NotEmpty("Tags are required.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.ErrorMessages.Count);
    }

    [Fact]
    public async Task IsNull_ShouldValidateNullValues()
    {
        // Arrange
        var model = new TestModel { Name = "not null" };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Name)
            .IsNull("Name must be null.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Name must be null.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task NotNull_ShouldValidateNonNullValues()
    {
        // Arrange
        var model = new TestModel { Name = null };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Name)
            .NotNull("Name is required.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Name is required.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task IsIn_ShouldValidateListMembership()
    {
        // Arrange
        var model = new TestModel { Status = "invalid" };
        var validator = new ValidatorBuilder<TestModel>();
        var validStatuses = new[] { "active", "inactive", "pending" };

        validator
            .RuleFor(x => x.Status)
            .IsIn(validStatuses, "Status must be one of: active, inactive, pending.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Status must be one of: active, inactive, pending.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task NotIn_ShouldValidateListNonMembership()
    {
        // Arrange
        var model = new TestModel { Status = "banned" };
        var validator = new ValidatorBuilder<TestModel>();
        var invalidStatuses = new[] { "banned", "suspended" };

        validator
            .RuleFor(x => x.Status)
            .NotIn(invalidStatuses, "Status must not be banned or suspended.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Status must not be banned or suspended.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task OneOf_ShouldValidateMultipleOptions()
    {
        // Arrange
        var model = new TestModel { Status = "invalid" };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Status)
            .OneOf(["active", "inactive"]);

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
    }

    [Fact]
    public async Task NoneOf_ShouldValidateAgainstMultipleOptions()
    {
        // Arrange
        var model = new TestModel { Status = "banned" };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Status)
            .NoneOf(["banned", "suspended"]);

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
    }

    [Fact]
    public async Task IsInEnum_ShouldValidateEnumValues()
    {
        // Arrange
        var model = new TestModel { Role = (UserRole)99 };
        var validator = new ValidatorBuilder<TestModel>();

        validator
            .RuleFor(x => x.Role)
            .IsInEnum("Role must be a valid enum value.");

        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Role must be a valid enum value.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public async Task CombinedValidations_ShouldWorkTogether()
    {
        // Arrange
        var model = new TestModel
        {
            Name = null,
            Age = 150,
            Status = "invalid",
            Tags = []
        };

        var validator = new ValidatorBuilder<TestModel>();
        validator
            .RuleFor(x => x.Name)
            .NotNull("Name is required.")
            .NotEmpty()
            .MaxLength(100);

        validator
            .RuleFor(x => x.Age)
            .NotNull();

        validator
            .RuleFor(x => x.Tags)
            .NotEmpty();


        validator
            .RuleFor(x => x.Status)
            .IsIn(["active", "inactive"], "Invalid status.");


        // Act
        var result = await validator.ValidateAsync(model, CancellationToken.None);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(4, result.ErrorMessages.Count);
    }
}

