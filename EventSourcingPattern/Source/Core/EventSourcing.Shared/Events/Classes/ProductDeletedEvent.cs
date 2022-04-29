using EventSource.Shared.Events.Interfaces;

namespace EventSource.Shared.Events.Classes
{
    public class ProductDeletedEvent : IEvent
    {
        public Guid Id { get; set; }
    }
}