using Microsoft.Extensions.DependencyInjection;
using WeChooz.TechAssessment.Application.Abstractions.Data;
using WeChooz.TechAssessment.Domain.Repositories;
using WeChooz.TechAssessment.Infrastructure.Data;
using WeChooz.TechAssessment.Infrastructure.Data.Repositories;

namespace WeChooz.TechAssessment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string formationConnectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new SqlConnectionFactory(formationConnectionString));
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        return services;
    }
}
