
using MatValidator;

var user = new User("", "janXd", 200);

Console.WriteLine("------------sample with builder---------");

var validator = new ValidatorBuilder<User>();

validator
    .RuleFor(x => x.FirstName)
    .NotEmpty()
    .MinLength(2);

validator
    .RuleFor(x => x.Email)
    .IsEmail();

validator.RuleFor(x => x.Age)
    .Range(1, 120);


var result = validator.Validate(user);
Console.WriteLine(result.IsValid);
Console.WriteLine(string.Join($",{Environment.NewLine}", result.ErrorMessages));

Console.WriteLine("------------sample with class---------");

var userValidator = new UserValidator();

result = userValidator.Validate(user);
Console.WriteLine(result.IsValid);
Console.WriteLine(string.Join($",{Environment.NewLine}", result.ErrorMessages));


public record User(string FirstName, string Email, int Age);

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
    }
}

