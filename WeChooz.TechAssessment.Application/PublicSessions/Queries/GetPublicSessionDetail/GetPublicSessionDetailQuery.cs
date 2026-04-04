using Shared.Mediator.Application;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessionDetail;

public sealed record GetPublicSessionDetailQuery(int SessionId) : IRequest<GetPublicSessionDetailResponse?>;
