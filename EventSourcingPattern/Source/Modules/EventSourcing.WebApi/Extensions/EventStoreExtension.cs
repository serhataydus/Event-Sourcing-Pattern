using EventSourcing.WebApi.EventStores;
using EventStore.ClientAPI;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace EventSourcing.WebApi.Extensions
{
    public static class EventStoreExtension
    {
        public static IServiceCollection AddEventStore(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            IEventStoreConnection connection = EventStoreConnection.Create(connectionString: configuration.GetConnectionString("EventStore"),
                                                                           connectionName: "EventSourcing.WebApi",
                                                                           builder: ConnectionSettings.Create().KeepReconnecting());

            connection.ConnectAsync().Wait();

            serviceCollection.AddSingleton(connection);

            using ILoggerFactory? logFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
            });

            ILogger? logger = logFactory.CreateLogger("Startup");

            connection.Connected += (sender, clientConnectionEventArgs) =>
            {
                logger.LogInformation("Bağlantı sağlanmıştır.");
                logger.LogInformation($"Connection Name  : {clientConnectionEventArgs.Connection.ConnectionName}");
                logger.LogInformation($"Address Family : {clientConnectionEventArgs.RemoteEndPoint.AddressFamily}");
            };

            connection.Disconnected += (sender, clientConnectionEventArgs) =>
            {
                logger.LogInformation("Bağlantı kesilmiştir.");
                logger.LogInformation($"Connection Name  : {clientConnectionEventArgs.Connection.ConnectionName}");
                logger.LogInformation($"Address Family : {clientConnectionEventArgs.RemoteEndPoint.AddressFamily}");
            };
            connection.Reconnecting += (sender, clientReconnectingEventArgs) =>
            {
                logger.LogInformation("Bağlantı yeniden deneniyor.");
                logger.LogInformation($"Connection Name  : {clientReconnectingEventArgs.Connection.ConnectionName}");
            };
            connection.ErrorOccurred += (sender, clientErrorEventArgs) =>
            {
                logger.LogError("Hata oluştu!.");
                logger.LogError($"Connection Name  : {clientErrorEventArgs.Connection.ConnectionName}");
                logger.LogError($"Exception Message : {clientErrorEventArgs.Exception.Message}");
            };

            serviceCollection.AddSingleton<ProductStream>();

            return serviceCollection;
        }
    }
}
