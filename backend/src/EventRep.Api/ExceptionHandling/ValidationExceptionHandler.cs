using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace EventRep.Api.ExceptionHandling;

internal sealed class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
            return false;

        var errors = validationException.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .Distinct()
                    .ToArray());

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await Results.ValidationProblem(errors)
            .ExecuteAsync(httpContext);

        return true;
    }
}
