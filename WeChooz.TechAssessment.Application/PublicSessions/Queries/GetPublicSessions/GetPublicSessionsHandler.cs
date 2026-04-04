using Shared.Mediator.Application;
using WeChooz.TechAssessment.Domain.ReadModels;
using WeChooz.TechAssessment.Domain.Repositories;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessions;

public sealed class GetPublicSessionsHandler(ISessionRepository sessions) : IRequestHandler<GetPublicSessionsQuery, GetPublicSessionsResponse>
{
    public async Task<GetPublicSessionsResponse> HandleAsync(GetPublicSessionsQuery request, CancellationToken cancellationToken = default)
    {
        var filter = new ListPublicSessionsFilter(
            request.Audience,
            request.DeliveryMode,
            request.StartAfter,
            request.StartBefore,
            request.StartFrom,
            request.StartTo);
        var rows = await sessions.ListPublicAsync(filter, cancellationToken);
        var items = rows.Select(r => r.ToItem()).ToList();
        return new GetPublicSessionsResponse(items);
    }
}
