using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace App.Services.ExceptionHandlers {
    public class GlobalExceptionHandler : IExceptionHandler {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {
            var error = ServiceResult.Fail(exception.Message, HttpStatusCode.InternalServerError);

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(error, cancellationToken)
                .ContinueWith(_ => true, cancellationToken);

            return true;
        }
    }
}