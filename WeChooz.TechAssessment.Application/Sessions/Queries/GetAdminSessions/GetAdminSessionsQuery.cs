using Shared.Mediator.Application;

namespace WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessions;

public sealed record GetAdminSessionsQuery : IRequest<GetAdminSessionsResponse>;
