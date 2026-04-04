using Shared.Mediator.Application;
using WeChooz.TechAssessment.Domain.Repositories;

namespace WeChooz.TechAssessment.Application.Participants.Queries.GetParticipantsBySession;

public sealed class GetParticipantsBySessionHandler(IParticipantRepository participants) : IRequestHandler<GetParticipantsBySessionQuery, GetParticipantsBySessionResponse>
{
    public async Task<GetParticipantsBySessionResponse> HandleAsync(GetParticipantsBySessionQuery request, CancellationToken cancellationToken = default)
    {
        var rows = await participants.ListBySessionAsync(request.SessionId, cancellationToken);
        var items = rows.Select(r => r.ToItem()).ToList();
        return new GetParticipantsBySessionResponse(items);
    }
}
