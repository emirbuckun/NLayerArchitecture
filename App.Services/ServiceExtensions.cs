using App.Services.Products;
using Microsoft.Extensions.DependencyInjection;

namespace App.Services {
    public static class ServiceExtensions {
        public static IServiceCollection AddServices(this IServiceCollection services) {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}