namespace MatValidator;
public partial class RuleBuilder<TModel, TProperty> : IValidationRule<TModel>
{
    public RuleBuilder<TModel, TProperty> When(Func<TModel, bool> condition)
    {
        _shouldValidate = condition ?? (_ => true);
        return this;
    }

    public RuleBuilder<TModel, TProperty> Unless(Func<TModel, bool> predicate)
    {
        _nextCondition = model => !predicate(model);
        return this;
    }

    public RuleBuilder<TModel, TProperty> If(Func<TModel, bool> condition)
    {
        _nextCondition = condition ?? (_ => true);
        return this;
    }
}
