using Shared.Mediator;
using WeChooz.TechAssessment.Application.Interfaces.Participants;

namespace WeChooz.TechAssessment.Application.Participants.Commands.AddParticipant;

public sealed class AddParticipantHandler(IParticipantRepository participants) : IRequestHandler<AddParticipantCommand, AddParticipantResponse>
{
    public async Task<AddParticipantResponse> HandleAsync(AddParticipantCommand request, CancellationToken cancellationToken = default)
    {
        var id = await participants.InsertAsync(
            request.SessionId,
            request.LastName,
            request.FirstName,
            request.Email,
            request.CompanyName,
            cancellationToken);
        return new AddParticipantResponse(id);
    }
}
