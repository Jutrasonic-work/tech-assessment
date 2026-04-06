using Shared.Mediator;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessionDetail;

public sealed record GetPublicSessionDetailQuery(int SessionId) : IRequest<GetPublicSessionDetailResponse?>;
