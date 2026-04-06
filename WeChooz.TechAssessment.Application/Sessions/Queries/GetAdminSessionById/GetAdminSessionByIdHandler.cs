using Shared.Mediator;
using WeChooz.TechAssessment.Application.Interfaces.Sessions;

namespace WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessionById;

public sealed class GetAdminSessionByIdHandler(ISessionRepository sessions) : IRequestHandler<GetAdminSessionByIdQuery, GetAdminSessionByIdResponse?>
{
    public async Task<GetAdminSessionByIdResponse?> HandleAsync(GetAdminSessionByIdQuery request, CancellationToken cancellationToken = default)
    {
        var row = await sessions.GetAdminByIdAsync(request.SessionId, cancellationToken);
        return row?.ToResponse();
    }
}
