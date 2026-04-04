using Shared.Mediator.Application;

namespace WeChooz.TechAssessment.Application.Courses.Commands.DeleteCourse;

public sealed record DeleteCourseCommand(int CourseId) : IRequest;
