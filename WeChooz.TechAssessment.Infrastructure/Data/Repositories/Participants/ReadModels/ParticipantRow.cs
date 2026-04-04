namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories.Participants.ReadModels;

internal sealed record ParticipantRow(int ParticipantId, int SessionId, string LastName, string FirstName, string Email, string CompanyName);
