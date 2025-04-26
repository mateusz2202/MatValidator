using MatValidator;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace MatValidatorTests;
public class RuleBuilderStringMethodsTests
{
    private record User(string FirstName, string Email, int Age, UserInfo UserInfo);
    private record UserInfo(string Info);

    private readonly ITestOutputHelper _output;
    public RuleBuilderStringMethodsTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact()]
    public void ValidateFalseTest()
    {
        var user = new User("", "janXd", 200, new(""));

        var validator = new ValidatorBuilder<User>();
        var validator2 = new ValidatorBuilder<UserInfo>();

        validator
            .RuleFor(x => x.FirstName)
            .NotEmpty()
            .Unless(x => true)
            .OverridePropertyName("First Name")
            .MinLength(2);

        validator
            .RuleFor(x => x.Email)
            .IsEmail();

        validator.RuleFor(x => x.Age)
            .Range(1, 120);

        validator2
            .RuleFor(x => x.Info)
            .NotEmpty()
            .MinLength(2);

        validator
            .RuleFor(x => x.UserInfo)
            .SetValidator(validator2);


        var result = validator.Validate(user);

        var jsonResult = JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray()));

        _output.WriteLine(jsonResult);

        Assert.False(result.IsValid);
    }

    [Fact()]
    public void ValidateTrueTest()
    {
        var user = new User("Jan", "jan@gmail.com", 35, new("info"));

        var validator = new ValidatorBuilder<User>();
        var validator2 = new ValidatorBuilder<UserInfo>();

        validator
            .RuleFor(x => x.FirstName)
            .NotEmpty()
            .Unless(x => true)
            .OverridePropertyName("First Name")
            .MinLength(2);

        validator
            .RuleFor(x => x.Email)
            .IsEmail();

        validator.RuleFor(x => x.Age)
            .Range(1, 120);

        validator2
            .RuleFor(x => x.Info)
            .NotEmpty()
            .MinLength(2);

        validator
            .RuleFor(x => x.UserInfo)
            .SetValidator(validator2);


        var result = validator.Validate(user);

        var jsonResult = JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray()));

        _output.WriteLine(jsonResult);

        Assert.True(result.IsValid);
    }
    [Fact]
    public void Length_ShouldValidateStringLength()
    {
        // Arrange
        var user = new User("Test", "test@example.com", 30, new UserInfo("info"));
        var validator = new ValidatorBuilder<User>();

        validator
            .RuleFor(x => x.FirstName)
            .Length(5, 10, "FirstName must be between 5 and 10 characters.");

        // Act
        var result = validator.Validate(user);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("FirstName must be between 5 and 10 characters.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public void MaxLength_ShouldValidateMaximumStringLength()
    {
        // Arrange
        var user = new User("TooLongName", "test@example.com", 30, new UserInfo("info"));
        var validator = new ValidatorBuilder<User>();

        validator
            .RuleFor(x => x.FirstName)
            .MaxLength(5, "FirstName must be at most 5 characters.");

        // Act
        var result = validator.Validate(user);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("FirstName must be at most 5 characters.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public void MinLength_ShouldValidateMinimumStringLength()
    {
        // Arrange
        var user = new User("A", "test@example.com", 30, new UserInfo("info"));
        var validator = new ValidatorBuilder<User>();

        validator
            .RuleFor(x => x.FirstName)
            .MinLength(2, "FirstName must be at least 2 characters.");

        // Act
        var result = validator.Validate(user);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("FirstName must be at least 2 characters.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public void IsEmail_ShouldValidateEmailFormat()
    {
        // Arrange
        var user = new User("John", "invalid-email", 30, new UserInfo("info"));
        var validator = new ValidatorBuilder<User>();

        validator
            .RuleFor(x => x.Email)
            .IsEmail("Invalid email format.");

        // Act
        var result = validator.Validate(user);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Invalid email format.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public void IsUrl_ShouldValidateUrlFormat()
    {
        // Arrange
        var user = new User("John", "test@example.com", 30, new UserInfo("invalid-url"));
        var validator = new ValidatorBuilder<User>();
        var userInfoValidator = new ValidatorBuilder<UserInfo>();

        userInfoValidator
            .RuleFor(x => x.Info)
            .IsUrl("Invalid URL format.");

        validator
            .RuleFor(x => x.UserInfo)
            .SetValidator(userInfoValidator);

        // Act
        var result = validator.Validate(user);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("Invalid URL format.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public void IsAlpha_ShouldValidateAlphabeticCharacters()
    {
        // Arrange
        var user = new User("John123", "test@example.com", 30, new UserInfo("info"));
        var validator = new ValidatorBuilder<User>();

        validator
            .RuleFor(x => x.FirstName)
            .IsAlpha("FirstName must contain only letters.");

        // Act
        var result = validator.Validate(user);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("FirstName must contain only letters.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public void IsAlphanumeric_ShouldValidateAlphanumericCharacters()
    {
        // Arrange
        var user = new User("John@Doe", "test@example.com", 30, new UserInfo("info"));
        var validator = new ValidatorBuilder<User>();

        validator
            .RuleFor(x => x.FirstName)
            .IsAlphanumeric("FirstName must be alphanumeric.");

        // Act
        var result = validator.Validate(user);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("FirstName must be alphanumeric.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public void StartsWith_ShouldValidateStringPrefix()
    {
        // Arrange
        var user = new User("JohnDoe", "test@example.com", 30, new UserInfo("info"));
        var validator = new ValidatorBuilder<User>();

        validator
            .RuleFor(x => x.FirstName)
            .StartsWith("Mr", "FirstName must start with 'Mr'.");

        // Act
        var result = validator.Validate(user);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("FirstName must start with 'Mr'.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public void EndsWith_ShouldValidateStringSuffix()
    {
        // Arrange
        var user = new User("JohnDoe", "test@example.com", 30, new UserInfo("info"));
        var validator = new ValidatorBuilder<User>();

        validator
            .RuleFor(x => x.FirstName)
            .EndsWith("Smith", "FirstName must end with 'Smith'.");

        // Act
        var result = validator.Validate(user);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("FirstName must end with 'Smith'.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public void Matches_ShouldValidateRegexPattern()
    {
        // Arrange
        var user = new User("JohnDoe", "test@example.com", 30, new UserInfo("info"));
        var validator = new ValidatorBuilder<User>();

        validator
            .RuleFor(x => x.FirstName)
            .Matches(@"^[A-Z][a-z]+$", "FirstName must start with capital letter followed by lowercase letters.");

        // Act
        var result = validator.Validate(user);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.ErrorMessages.ToArray());
        Assert.Equal("FirstName must start with capital letter followed by lowercase letters.", result.ErrorMessages.ToArray()[0]);
    }

    [Fact]
    public void CombinedValidations_ShouldWorkTogether()
    {
        // Arrange
        var user = new User("A", "invalid-email", 200, new UserInfo(""));
        var validator = new ValidatorBuilder<User>();
        var userInfoValidator = new ValidatorBuilder<UserInfo>();

        validator
            .RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinLength(2, "First Name must be at least 2 characters.");

        validator
            .RuleFor(x => x.Email)
            .IsEmail("Invalid email format.");

        validator.RuleFor(x => x.Age)
            .Range(1, 120);

        userInfoValidator
            .RuleFor(x => x.Info)
            .NotEmpty()
            .MinLength(2);

        validator
            .RuleFor(x => x.UserInfo)
            .SetValidator(userInfoValidator);

        // Act
        var result = validator.Validate(user);
        _output.WriteLine(JsonSerializer.Serialize((result.IsValid, result.ErrorMessages.ToArray())));

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(5, result.ErrorMessages.Count);
    }
}
