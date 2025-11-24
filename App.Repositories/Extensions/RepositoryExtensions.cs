using App.Repositories.Categories;
using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App.Repositories.Extensions {
    public static class RepositoryExtensions {
        public static IServiceCollection AddRepositories(this IServiceCollection services) {
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("AppDatabase"));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}