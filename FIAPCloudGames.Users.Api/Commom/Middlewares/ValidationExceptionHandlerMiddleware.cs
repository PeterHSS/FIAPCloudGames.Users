using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace FIAPCloudGames.Users.Api.Commom.Middlewares;

internal sealed class ValidationExceptionHandlerMiddleware(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new()
            {
                Detail = "One or more validation errors ocorrued.",
                Status = StatusCodes.Status400BadRequest,
            }
        };

        var errors = validationException.Errors
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                failureGroup => failureGroup.Key.ToLowerInvariant(), 
                failureGroup => failureGroup.Select(e => e.ErrorMessage).ToArray()
            );

        context.ProblemDetails.Extensions.Add("errors", errors);

        return await problemDetailsService.TryWriteAsync(context);
    }
}
