using EventSource.Shared.Events.Interfaces;

namespace EventSource.Shared.Events.Classes
{
    public class ProductPriceChangedEvent : IEvent
    {
        public Guid Id { get; set; }
        public decimal ChangedPrice { get; set; }
    }
}