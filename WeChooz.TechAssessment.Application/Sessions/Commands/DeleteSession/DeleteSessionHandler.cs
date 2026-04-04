using Shared.Mediator.Application;
using WeChooz.TechAssessment.Domain.Repositories;

namespace WeChooz.TechAssessment.Application.Sessions.Commands.DeleteSession;

public sealed class DeleteSessionHandler(ISessionRepository sessions) : IRequestHandler<DeleteSessionCommand>
{
    public Task HandleAsync(DeleteSessionCommand request, CancellationToken cancellationToken = default) =>
        sessions.DeleteAsync(request.SessionId, cancellationToken);
}
