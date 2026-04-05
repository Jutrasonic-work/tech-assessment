using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Mediator.Application;
using WeChooz.TechAssessment.Application.Behaviors;

namespace WeChooz.TechAssessment.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Singleton);
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<>), typeof(VoidRequestValidationBehavior<>));
        return services;
    }
}
