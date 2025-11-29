using App.Domain;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.ServiceBus {
    public static class ServiceBusExtensions {
        public static IServiceCollection AddServiceBusExtensions(this IServiceCollection services, IConfiguration configuration) {
            var serviceBusOptions = configuration.GetSection(nameof(ServiceBusOptions)).Get<ServiceBusOptions>();
            services.AddMassTransit(x => {
                x.UsingRabbitMq((context, cfg) => {
                    cfg.Host(new Uri(serviceBusOptions!.Url), h => { });
                });
            });
            return services;
        }
    }
}