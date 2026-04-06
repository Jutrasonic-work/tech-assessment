using Microsoft.Extensions.DependencyInjection;
using Shared.Mediator;
using Shared.Validation;

namespace WeChooz.TechAssessment.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator();
        services.AddValidation();
        return services;
    }
}
