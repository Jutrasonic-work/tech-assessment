using FluentValidation;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourseById;

public sealed class GetCourseByIdQueryValidator : AbstractValidator<GetCourseByIdQuery>
{
    public GetCourseByIdQueryValidator()
    {
        RuleFor(x => x.CourseId).GreaterThan(0);
    }
}
