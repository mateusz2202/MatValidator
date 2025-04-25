namespace MatValidator;
public sealed partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    public RuleBuilder<TModel, TProperty> When(Func<TModel, bool> condition)
    {
        ShouldValidate = condition ?? (_ => true);
        return this;
    }

    public RuleBuilder<TModel, TProperty> Unless(Func<TModel, bool> predicate)
    {
        NextCondition = model => !predicate(model);
        return this;
    }

    public RuleBuilder<TModel, TProperty> If(Func<TModel, bool> condition)
    {
        NextCondition = condition ?? (_ => true);
        return this;
    }
}
