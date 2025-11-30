namespace App.Domain.Events {
    public record ProductCreatedEvent(int Id, string Name, decimal Price) : IEventOrMessage;
}