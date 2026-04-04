using Shared.Mediator.Application;
using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessions;

public sealed record GetPublicSessionsQuery(
    CseAudience? Audience,
    SessionDeliveryMode? DeliveryMode,
    DateTime? StartAfter,
    DateTime? StartBefore,
    DateTime? StartFrom,
    DateTime? StartTo) : IRequest<GetPublicSessionsResponse>;
