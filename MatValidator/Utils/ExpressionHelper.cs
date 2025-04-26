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
        if (expression.Body is MemberExpression memberExpr)
        {
            return memberExpr.Member is not PropertyInfo propInfo
                ? throw new InvalidOperationException("Expression does not refer to a property.")
                : propInfo.GetValue(instance);
        }

        if (expression.Body is UnaryExpression unaryExpr && unaryExpr.Operand is MemberExpression unaryMemberExpr)
        {
            return unaryMemberExpr.Member is not PropertyInfo propInfo
                ? throw new InvalidOperationException("Expression does not refer to a property.")
                : propInfo.GetValue(instance);
        }

        throw new ArgumentException("Invalid expression form");
    }
}
