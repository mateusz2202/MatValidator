
using MatValidator;

var user = new User("Jan", "janXd", 200);


var validator = new ValidatorBuilder();

validator
    .RuleFor(user, x => x.FirstName)
    .NotEmpty()
    .MinLength(2);

validator
    .RuleFor(user, x => x.Email)
    .IsEmail();

validator.RuleFor(user, x => x.Age)
    .Range(1, 120);


var result = validator.Validate();

Console.WriteLine(result.IsValid);
Console.WriteLine(string.Join($",{Environment.NewLine}", result.ErrorMessages));


public record User(string FirstName, string Email, int Age);

