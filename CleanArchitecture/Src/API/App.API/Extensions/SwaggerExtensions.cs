namespace App.API.Extensions {
    public static class SwaggerExtensions {
        public static IServiceCollection AddSwaggerExtensions(this IServiceCollection services) {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(
                options => {
                    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
                        Title = "Clean Architecture API",
                        Version = "v1"
                    });
                }
            );
            return services;
        }

        public static IApplicationBuilder UseSwaggerExtensions(this IApplicationBuilder app) {
            app.UseSwagger();
            app.UseSwaggerUI(
                options => {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Architecture API v1");
                    options.RoutePrefix = string.Empty;
                }
            );
            return app;
        }
    }
}