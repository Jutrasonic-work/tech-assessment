using Shared.Mediator.Application;
using WeChooz.TechAssessment.Domain.Repositories;

namespace WeChooz.TechAssessment.Application.Participants.Commands.RemoveParticipant;

public sealed class RemoveParticipantHandler(IParticipantRepository participants) : IRequestHandler<RemoveParticipantCommand>
{
    public Task HandleAsync(RemoveParticipantCommand request, CancellationToken cancellationToken = default) =>
        participants.DeleteAsync(request.ParticipantId, cancellationToken);
}
