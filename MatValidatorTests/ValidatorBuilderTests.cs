using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace MatValidator.Tests;

public class ValidatorBuilderTests
{
    private readonly ITestOutputHelper _output;
    public ValidatorBuilderTests(ITestOutputHelper output)
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

        var jsonResult = JsonSerializer.Serialize(result);

        _output.WriteLine(jsonResult);

        Xunit.Assert.False(result.IsValid);
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

        var jsonResult = JsonSerializer.Serialize(result);

        _output.WriteLine(jsonResult);

        Xunit.Assert.True(result.IsValid);
    }
}

public record User(string FirstName, string Email, int Age, UserInfo UserInfo);
public record UserInfo(string Info);