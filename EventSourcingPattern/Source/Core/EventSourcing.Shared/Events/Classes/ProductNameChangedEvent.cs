using EventSource.Shared.Events.Interfaces;

namespace EventSource.Shared.Events.Classes
{
    public class ProductNameChangedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string ChangedName { get; set; }
    }
}