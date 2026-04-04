using Shared.Mediator.Application;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Application.Sessions.Commands.UpdateSession;

public sealed record UpdateSessionCommand(int SessionId, int CourseId, DateTime StartDate, SessionDeliveryMode DeliveryMode) : IRequest<UpdateSessionResponse>;
