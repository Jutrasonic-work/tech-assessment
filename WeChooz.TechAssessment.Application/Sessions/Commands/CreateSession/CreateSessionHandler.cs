using Shared.Mediator;
using WeChooz.TechAssessment.Application.Interfaces.Sessions;

namespace WeChooz.TechAssessment.Application.Sessions.Commands.CreateSession;

public sealed class CreateSessionHandler(ISessionRepository sessions) : IRequestHandler<CreateSessionCommand, CreateSessionResponse>
{
    public async Task<CreateSessionResponse> HandleAsync(CreateSessionCommand request, CancellationToken cancellationToken = default)
    {
        var id = await sessions.InsertAsync(request.CourseId, request.StartDate, request.DeliveryMode, cancellationToken);
        return new CreateSessionResponse(id);
    }
}
