namespace MatValidator;
public sealed partial class RuleBuilder<TModel, TProperty> : IValidatorRule
{
    public RuleBuilder<TModel, TProperty> When(Func<TModel, bool> condition)
    {
        ShouldValidate = new Predicate<TModel>(condition ?? (static _ => true));
        return this;
    }

    public RuleBuilder<TModel, TProperty> Unless(Func<TModel, bool> predicate)
    {
        NextCondition = new Predicate<TModel>(model => !predicate(model));
        return this;
    }

    public RuleBuilder<TModel, TProperty> If(Func<TModel, bool> condition)
    {
        NextCondition = new Predicate<TModel>(condition ?? (static _ => true));
        return this;
    }
}
