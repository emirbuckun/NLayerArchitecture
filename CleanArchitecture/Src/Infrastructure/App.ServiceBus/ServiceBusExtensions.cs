using App.Application.Contracts.ServiceBus;
using App.Domain;
using App.Domain.Consts;
using App.Infrastructure.App.ServiceBus.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.ServiceBus {
    public static class ServiceBusExtensions {
        public static IServiceCollection AddServiceBusExtensions(this IServiceCollection services, IConfiguration configuration) {
            services.AddScoped<IServiceBus, ServiceBus>();

            var serviceBusOptions = configuration.GetSection(nameof(ServiceBusOptions)).Get<ServiceBusOptions>();

            services.AddMassTransit(x => {
                x.AddConsumer<ProductCreatedEventConsumer>();
                x.UsingRabbitMq((context, cfg) => {
                    cfg.Host(new Uri(serviceBusOptions!.Url), h => { });
                    cfg.ReceiveEndpoint(ServiceBusConsts.ProductCreatedEventQueueName, e => {
                        e.ConfigureConsumer<ProductCreatedEventConsumer>(context);
                    });
                });
            });

            return services;
        }
    }
}