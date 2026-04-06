using Shared.Mediator;
using WeChooz.TechAssessment.Application.Interfaces.Sessions;

namespace WeChooz.TechAssessment.Application.Sessions.Commands.DeleteSession;

public sealed class DeleteSessionHandler(ISessionRepository sessions) : IRequestHandler<DeleteSessionCommand>
{
    public Task HandleAsync(DeleteSessionCommand request, CancellationToken cancellationToken = default) =>
        sessions.DeleteAsync(request.SessionId, cancellationToken);
}
