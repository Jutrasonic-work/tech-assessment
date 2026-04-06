using Microsoft.Extensions.DependencyInjection;
using WeChooz.TechAssessment.Application.Interfaces.Courses;
using WeChooz.TechAssessment.Application.Interfaces.Participants;
using WeChooz.TechAssessment.Application.Interfaces.Sessions;
using WeChooz.TechAssessment.Infrastructure.Data;
using WeChooz.TechAssessment.Infrastructure.Data.Repositories.Courses;
using WeChooz.TechAssessment.Infrastructure.Data.Repositories.Participants;
using WeChooz.TechAssessment.Infrastructure.Data.Repositories.Sessions;

namespace WeChooz.TechAssessment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string formationConnectionString)
    {
        DapperTypeHandlers.Register();

        services.AddSingleton<IDbConnectionFactory>(_ => new SqlConnectionFactory(formationConnectionString));
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        return services;
    }
}
