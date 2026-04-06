namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories.Participants.ReadModels;

internal sealed record ParticipantRead(int ParticipantId, int SessionId, string LastName, string FirstName, string Email, string CompanyName);
