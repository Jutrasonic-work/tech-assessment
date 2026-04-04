using Shared.Mediator.Application;
using WeChooz.TechAssessment.Domain.Repositories;

namespace WeChooz.TechAssessment.Application.Participants.Commands.UpdateParticipant;

public sealed class UpdateParticipantHandler(IParticipantRepository participants) : IRequestHandler<UpdateParticipantCommand, UpdateParticipantResponse>
{
    public async Task<UpdateParticipantResponse> HandleAsync(UpdateParticipantCommand request, CancellationToken cancellationToken = default)
    {
        var updated = await participants.UpdateAsync(
            request.ParticipantId,
            request.SessionId,
            request.LastName,
            request.FirstName,
            request.Email,
            request.CompanyName,
            cancellationToken);
        return new UpdateParticipantResponse(updated);
    }
}
