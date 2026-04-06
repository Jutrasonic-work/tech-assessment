using Shared.Mediator;

namespace WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessionById;

public sealed record GetAdminSessionByIdQuery(int SessionId) : IRequest<GetAdminSessionByIdResponse?>;
