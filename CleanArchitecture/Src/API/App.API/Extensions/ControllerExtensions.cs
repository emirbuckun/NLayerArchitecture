using App.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Extensions {
    public static class ControllerExtensions {
        public static IServiceCollection AddControllerExtensions(this IServiceCollection services) {
            services.AddControllers(options => {
                options.Filters.Add<FluentValidationFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            services.Configure<ApiBehaviorOptions>(options => {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped(typeof(NotFoundFilter<,>));

            return services;
        }
    }
}