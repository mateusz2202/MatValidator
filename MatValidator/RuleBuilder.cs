namespace MatValidator;

internal interface IValidationRule<TModel>
{
    Func<TModel, bool> ShouldValidate { get; }
    Func<TModel, bool> NextCondition { get; }
    int Id { get; }
}

public sealed partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    private readonly ValidatorBuilder<TModel> _parent;
    public Func<TModel, bool> ShouldValidate { get; private set; } = _ => true;
    public Func<TModel, bool> NextCondition { get; private set; } = _ => true;


    private string _propertyName;

    public int Id { get; init; }


    public RuleBuilder(ValidatorBuilder<TModel> parent, string propertyName, int id)
    {
        _parent = parent;
        _propertyName = propertyName;
        Id = id;
    }

    private RuleBuilder<TModel, TProperty> AddValidator(Func<object, string> validator)
    {
        _parent.AddValidator(new ValidatorRule<TModel>(NextCondition, validator, Id));

        NextCondition = _ => true;

        return this;
    }
}