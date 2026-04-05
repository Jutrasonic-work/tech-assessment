using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Reflection;

namespace Shared.Validation.Api;

/// <summary>
/// Indicates that a parameter should be validated using FluentValidation.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class ValidateAttribute : Attribute
{
}

public static class ValidationFilter
{
    /// <summary>
    /// An endpoint filter factory that performs validation on parameters decorated with the <see cref="ValidateAttribute"/>.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public static EndpointFilterDelegate ValidationFilterFactory(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
    {
        IEnumerable<ValidationDescriptor> validationDescriptors = GetValidators(context.MethodInfo, context.ApplicationServices);

        if (validationDescriptors.Any())
        {
            return invocationContext =>
            {
                return Validate(validationDescriptors, invocationContext, next);
            };
        }

        // pass-thru
        return invocationContext => next(invocationContext);
    }

    /// <summary>
    /// Validates the parameters of the endpoint using the provided validators.
    /// </summary>
    /// <param name="validationDescriptors"></param>
    /// <param name="invocationContext"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    private static async ValueTask<object?> Validate(IEnumerable<ValidationDescriptor> validationDescriptors, EndpointFilterInvocationContext invocationContext, EndpointFilterDelegate next)
    {
        foreach (ValidationDescriptor descriptor in validationDescriptors)
        {
            var argument = invocationContext.Arguments[descriptor.ArgumentIndex];
            if (argument is null)
            {
                // Objet manquant → retourne une ValidationProblem
                return Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        [descriptor.ArgumentType.Name] = ["Request body is required."]
                    },
                    statusCode: (int)HttpStatusCode.UnprocessableEntity
                );
            }
            var validationResult = await descriptor.Validator.ValidateAsync(
                new ValidationContext<object>(argument)
            );

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary(),
                    statusCode: (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        return await next.Invoke(invocationContext);
    }

    /// <summary>
    /// Retrieves validators for parameters decorated with the <see cref="ValidateAttribute"/>.
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    static IEnumerable<ValidationDescriptor> GetValidators(MethodInfo methodInfo, IServiceProvider serviceProvider)
    {
        ParameterInfo[] parameters = methodInfo.GetParameters();

        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterInfo parameter = parameters[i];

            if (parameter.GetCustomAttribute<ValidateAttribute>() is not null)
            {
                Type validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

                // Note that FluentValidation validators needs to be registered as singleton

                IValidator? validator = serviceProvider.GetService(validatorType) as IValidator;

                if (validator is not null)
                {
                    yield return new ValidationDescriptor { ArgumentIndex = i, ArgumentType = parameter.ParameterType, Validator = validator };
                }
            }
        }
    }

    /// <summary>
    /// Descriptor for a parameter validator.
    /// </summary>
    private class ValidationDescriptor
    {
        public required int ArgumentIndex { get; init; }
        public required Type ArgumentType { get; init; }
        public required IValidator Validator { get; init; }
    }

}