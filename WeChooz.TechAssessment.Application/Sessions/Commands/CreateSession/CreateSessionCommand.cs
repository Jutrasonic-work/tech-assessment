using Shared.Mediator.Application;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Application.Sessions.Commands.CreateSession;

public sealed record CreateSessionCommand(int CourseId, DateTime StartDate, SessionDeliveryMode DeliveryMode) : IRequest<CreateSessionResponse>;
