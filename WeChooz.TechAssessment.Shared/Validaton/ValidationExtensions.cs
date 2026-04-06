using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Validation;

public static class ValidationExtensions
{
    /// <summary>
    /// Add FluentValidation validators from the calling assembly to the service collection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        // Tip : Assumes the entry assembly is the API project and the validators are in the corresponding Application project
        var entryAssembly = Assembly.GetEntryAssembly();
        var assemblyName = entryAssembly!.GetName().Name!
            .Replace(".Api", ".Application", StringComparison.Ordinal)
            .Replace(".Web", ".Application", StringComparison.Ordinal)
            .Replace(".AppHost", ".Application", StringComparison.Ordinal)
            .Replace(".Console", ".Application", StringComparison.Ordinal);
        var assembly = Assembly.Load(assemblyName);

        // Register all validators as singletons
        services.AddValidatorsFromAssembly(assembly, ServiceLifetime.Singleton);
        return services;
    }

}
