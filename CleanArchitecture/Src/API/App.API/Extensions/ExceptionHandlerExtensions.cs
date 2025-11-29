using App.API.ExceptionHandlers;

namespace App.API.Extensions {
    public static class ExceptionHandlerExtensions {
        public static IServiceCollection AddExceptionHandlerExtensions(this IServiceCollection services) {
            services.AddExceptionHandler<CriticalExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }
    }
}