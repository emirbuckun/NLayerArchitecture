namespace App.API.Extensions {
    public static class ConfigurePipelineExtensions {
        public static IApplicationBuilder ConfigurePipeline(this IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseSwaggerExtensions();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            return app;
        }
    }
}