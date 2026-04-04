using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Mediator.Application;

public static class MediatorExtensions
{
    /// <summary>
    /// Adds the mediator and all request handlers from the application assembly to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        var openInterfaces = new[]
        {
            typeof(IRequestHandler<>),
            typeof(IRequestHandler<,>)
        };

        services.AddScoped<IMediator, Mediator>();

        // Tip : Assumes the entry assembly is the API project and the validators are in the corresponding Application project
        var entryAssembly = Assembly.GetEntryAssembly();
        var assemblyName = entryAssembly!.GetName().Name!
            .Replace(".Api", ".Application", StringComparison.Ordinal)
            .Replace(".Web", ".Application", StringComparison.Ordinal)
            .Replace(".AppHost", ".Application", StringComparison.Ordinal)
            .Replace(".Console", ".Application", StringComparison.Ordinal);
        var assembly = Assembly.Load(assemblyName);

        // Register all request handlers
        var types = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && openInterfaces.Contains(i.GetGenericTypeDefinition()))
                .Select(i => new { Handler = t, Interface = i }));

        // Register each handler with its interface
        foreach (var type in types)
        {
            services.AddTransient(type.Interface, type.Handler);
        }

        return services;
    }
}
