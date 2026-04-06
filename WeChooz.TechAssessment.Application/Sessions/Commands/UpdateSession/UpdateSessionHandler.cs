using Shared.Mediator;
using WeChooz.TechAssessment.Application.Interfaces.Sessions;

namespace WeChooz.TechAssessment.Application.Sessions.Commands.UpdateSession;

public sealed class UpdateSessionHandler(ISessionRepository sessions) : IRequestHandler<UpdateSessionCommand, UpdateSessionResponse>
{
    public async Task<UpdateSessionResponse> HandleAsync(UpdateSessionCommand request, CancellationToken cancellationToken = default)
    {
        var updated = await sessions.UpdateAsync(request.SessionId, request.CourseId, request.StartDate, request.DeliveryMode, cancellationToken);
        return new UpdateSessionResponse(updated);
    }
}
