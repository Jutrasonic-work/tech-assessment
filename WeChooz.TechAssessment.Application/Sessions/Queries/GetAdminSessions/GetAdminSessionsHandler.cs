using Shared.Mediator.Application;
using WeChooz.TechAssessment.Domain.Repositories;

namespace WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessions;

public sealed class GetAdminSessionsHandler(ISessionRepository sessions) : IRequestHandler<GetAdminSessionsQuery, GetAdminSessionsResponse>
{
    public async Task<GetAdminSessionsResponse> HandleAsync(GetAdminSessionsQuery request, CancellationToken cancellationToken = default)
    {
        var rows = await sessions.ListAdminAsync(cancellationToken);
        var items = rows.Select(r => r.ToItem()).ToList();
        return new GetAdminSessionsResponse(items);
    }
}
