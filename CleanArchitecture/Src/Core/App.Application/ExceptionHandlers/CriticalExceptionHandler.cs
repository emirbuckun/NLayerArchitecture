using App.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace App.Application.ExceptionHandlers {
    public class CriticalExceptionHandler : IExceptionHandler {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {
            if (exception is CriticalException) {
                Console.WriteLine("A critical exception has been handled.");
            }

            return ValueTask.FromResult(false);
        }
    }
}