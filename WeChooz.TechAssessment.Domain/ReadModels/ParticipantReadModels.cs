namespace WeChooz.TechAssessment.Domain.ReadModels;

public sealed record ParticipantResult(int ParticipantId, int SessionId, string LastName, string FirstName, string Email, string CompanyName);
