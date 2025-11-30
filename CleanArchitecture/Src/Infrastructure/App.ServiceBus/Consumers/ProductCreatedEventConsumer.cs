using App.Domain.Events;
using MassTransit;

namespace App.Infrastructure.App.ServiceBus.Consumers {
    public class ProductCreatedEventConsumer : IConsumer<ProductCreatedEvent> {
        public Task Consume(ConsumeContext<ProductCreatedEvent> context) {
            Console.WriteLine($"Product Created Event Consumed: Id={context.Message.Id}, Name={context.Message.Name}, Price={context.Message.Price}");
            return Task.CompletedTask;
        }
    }
}