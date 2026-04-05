using FluentValidation;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourses;

public sealed class GetCoursesQueryValidator : AbstractValidator<GetCoursesQuery>
{
    public GetCoursesQueryValidator()
    {
        // Requête sans paramètres : règles métier éventuelles plus tard.
    }
}
