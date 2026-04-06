using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Shared.Validation;

public static class FluentValidationExceptionExtensions
{
    /// <summary>
    /// Retourne un 422 ValidationProblem lorsqu'une <see cref="ValidationException"/> remonte depuis le pipeline (médiateur, handlers, etc.).
    /// </summary>
    public static IApplicationBuilder UseFluentValidationExceptionHandler(this IApplicationBuilder app) =>
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                await Results.ValidationProblem(errors).ExecuteAsync(context);
            }
        });
}
