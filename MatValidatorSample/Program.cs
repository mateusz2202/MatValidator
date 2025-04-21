
using MatValidator;

var user = new User("Jan", "janXd", 200);


var validator = new ValidatorBuilder();

validator.AddRule().NotEmpty(Property.Of(user, x => x.Email));
validator.AddRule().MinLength(Property.Of(user.FirstName), 2);
validator.AddRule().IsEmail(Property.Of(user, user => user.Email));
validator.AddRule().Range(Property.Of(user.Age), 1, 120);




var result = validator.Validate();

Console.WriteLine(result.IsValid);
Console.WriteLine(string.Join($",{Environment.NewLine}", result.ErrorMessages));


public record User(string FirstName, string Email, int Age);

