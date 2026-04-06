namespace WeChooz.TechAssessment.Web.Api;

public static class ApiEndpoints
{
    public static WebApplication MapApi(this WebApplication app)
    {
        var api = app.MapGroup("/api").DisableAntiforgery();

        api.MapAuthEndpoints();
        api.MapCourseEndpoints();
        api.MapSessionEndpoints();
        api.MapParticipantEndpoints();

        return app;
    }
}
