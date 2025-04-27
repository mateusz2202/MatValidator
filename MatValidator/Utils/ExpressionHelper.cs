using System.Linq.Expressions;
using System.Reflection;

namespace MatValidator.Utils;
public static class ExpressionHelper
{
    public static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> expression)
    {
        if (expression.Body is MemberExpression memberExpr)
            return memberExpr.Member.Name;

        if (expression.Body is UnaryExpression unaryExpr && unaryExpr.Operand is MemberExpression unaryMember)
            return unaryMember.Member.Name;

        throw new ArgumentException("Invalid expression form");
    }

    public static object GetPropertyValue<T, TProperty>(this T instance, Expression<Func<T, TProperty>> expression)
    {
        if (expression.Body is not MemberExpression memberExpr)
            throw new InvalidOperationException("Expression must be a member access expression.");

        var memberNames = new Stack<string>();
        var currentExpr = memberExpr;

        while (currentExpr != null)
        {
            if (currentExpr.Member is not PropertyInfo propInfo)
                throw new InvalidOperationException("Expression does not refer to a property.");

            memberNames.Push(propInfo.Name);
            currentExpr = currentExpr.Expression as MemberExpression;
        }

        object currentValue = instance;
        foreach (var name in memberNames)
        {
            if (currentValue == null)
                return null;

            var prop = currentValue.GetType().GetProperty(name);
            currentValue = prop.GetValue(currentValue);
        }

        return currentValue;
    }
}
