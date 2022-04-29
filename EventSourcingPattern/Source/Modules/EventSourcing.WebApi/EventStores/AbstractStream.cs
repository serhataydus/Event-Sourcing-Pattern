using EventSource.Shared.Events.Interfaces;
using EventStore.ClientAPI;
using System.Text;
using System.Text.Json;

namespace EventSourcing.WebApi.EventStores
{
    public abstract class AbstractStream
    {
        protected readonly LinkedList<IEvent> Events = new();
        private readonly IEventStoreConnection _eventStoreConnection;
        private string _streamName { get; }

        public AbstractStream(string streamName, IEventStoreConnection eventStoreConnection)
        {
            _streamName = streamName;
            _eventStoreConnection = eventStoreConnection;
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            var newEvents = Events.ToList().Select(x => new EventData(
                 Guid.NewGuid(),
                 x.GetType().Name,
                 true,
                 Encoding.UTF8.GetBytes(JsonSerializer.Serialize(x, inputType: x.GetType())),
                 Encoding.UTF8.GetBytes(x.GetType().FullName))).ToList();

            await _eventStoreConnection.AppendToStreamAsync(_streamName, ExpectedVersion.Any, newEvents);

            Events.Clear();
        }
    }
}
