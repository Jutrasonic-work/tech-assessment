using Shared.Mediator;
using WeChooz.TechAssessment.Application.Interfaces.Participants;

namespace WeChooz.TechAssessment.Application.Participants.Commands.RemoveParticipant;

public sealed class RemoveParticipantHandler(IParticipantRepository participants) : IRequestHandler<RemoveParticipantCommand>
{
    public Task HandleAsync(RemoveParticipantCommand request, CancellationToken cancellationToken = default) =>
        participants.DeleteAsync(request.ParticipantId, cancellationToken);
}
