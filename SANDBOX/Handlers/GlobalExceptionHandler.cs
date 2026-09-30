using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SANDBOX.Exceptions;

namespace SANDBOX.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                NotFoundException
                        => StatusCodes.Status404NotFound,

                ConflictException
                        => StatusCodes.Status409Conflict,

                UnauthorizedException
                        => StatusCodes.Status401Unauthorized,

                _ => StatusCodes.Status500InternalServerError
            };

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = GetTitle(statusCode),
                Detail = 
                    statusCode == StatusCodes.Status500InternalServerError
                        ? "An unexpected error occurred."
                        : exception.Message
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(
                problem,
                cancellationToken
            );

            return true;
        }
        private static string GetTitle(int statusCode)
        {
            return statusCode switch
            {
                404 => "Not Found",
                409 => "Conflict",
                401 => "Unauthorized",
                _ => "Internal Server Error"
            };
        }
    }
}
