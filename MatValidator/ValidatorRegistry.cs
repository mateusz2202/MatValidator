using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MatValidator;
public static class ValidatorRegistry
{
    public static IServiceCollection AddMatValidators(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        if (assemblies.Length == 0)
            assemblies = [Assembly.GetCallingAssembly()];


        var validatorTypes = assemblies.SelectMany(x => x.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface &&
                        t.BaseType != null &&
                        t.BaseType.IsGenericType &&
                        t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>)));

        foreach (var validatorType in validatorTypes)
            services.AddScoped(typeof(AbstractValidator<>).MakeGenericType(validatorType.BaseType!.GetGenericArguments()[0]), validatorType);

        return services;
    }
}
