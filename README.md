# MatValidator  

MatValidator is a .NET 9 library designed to validate mathematical expressions and ensure they conform to specific rules or formats.  

## Features  
- Validate mathematical expressions for syntax correctness.  
- Support for custom validation rules.  
- Lightweight and easy to integrate into .NET applications.  

## Features  
- Validate mathematical expressions for syntax correctness.  
- Support for custom validation rules.  
- Lightweight and easy to integrate into .NET applications.  

### Custom Validation Rules  

You can define custom validation rules to extend the functionality of MatValidator.  

### Sample
```csharp
using MatValidator;

var user = new User("", "janXd", 200, new(""));

Console.WriteLine("------------sample with builder---------");

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
Console.WriteLine(result.IsValid);
Console.WriteLine(string.Join($",{Environment.NewLine}", result.ErrorMessages));

Console.WriteLine("------------sample with class---------");

var userValidator = new UserValidator();

result = userValidator.Validate(user);
Console.WriteLine(result.IsValid);
Console.WriteLine(string.Join($",{Environment.NewLine}", result.ErrorMessages));


public record User(string FirstName, string Email, int Age, UserInfo UserInfo);
public record UserInfo(string Info);


public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinLength(2);

        RuleFor(x => x.Email)
            .IsEmail();

        RuleFor(x => x.Age)
            .Range(1, 120);

        RuleFor(x => x.UserInfo)
            .SetValidator(new UserInfoValidator());
    }
}


public class UserInfoValidator : AbstractValidator<UserInfo>
{
    public UserInfoValidator()
    {
        RuleFor(x => x.Info)
            .NotEmpty()
            .MinLength(2);
    }
}
```
## License  

This project is licensed under the MIT License.
