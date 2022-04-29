using EventSource.Shared.Events.Classes;
using EventSourcing.WebApi.Data;
using EventSourcing.WebApi.Data.Entities;
using EventSourcing.WebApi.EventStores;
using EventStore.ClientAPI;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace EventSourcing.WebApi.BackgroundServices
{
    public class ProductReadModelEventStore : BackgroundService
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly ILogger<ProductReadModelEventStore> _logger;

        // Not : BackgroundService Singleton olduğu için DBContext'te scope olduğu için BackgroundService DBContext'te erişemez.
        private readonly IServiceProvider _serviceProvider;

        public ProductReadModelEventStore(IEventStoreConnection eventStoreConnection, ILogger<ProductReadModelEventStore> logger, IServiceProvider serviceProvider)
        {
            _eventStoreConnection = eventStoreConnection;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("BackgroundService Started ...");

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _eventStoreConnection.ConnectToPersistentSubscriptionAsync(ProductStream.ProductName, ProductStream.SreamGroupName, EventAppeared, autoAck: false);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("BackgroundService Stoped ...");

            return base.StopAsync(cancellationToken);
        }

        private async Task EventAppeared(EventStorePersistentSubscriptionBase eventStorePersistentSubscriptionBase, ResolvedEvent resolvedEvent)
        {
            var type = Type.GetType($"{Encoding.UTF8.GetString(resolvedEvent.Event.Metadata)}, EventSourcing.Shared"); // ", EventSourcing.Shared" Ayrı bir class library'de olduğu için ismini yazıyoruz.

            _logger.LogInformation($"The message process ... : {type.ToString()}");

            string? eventData = Encoding.UTF8.GetString(resolvedEvent.Event.Data);

            object? @event = JsonSerializer.Deserialize(eventData, type);

            using IServiceScope? scope = _serviceProvider.CreateScope();
            IProductDbContext? context = scope.ServiceProvider.GetService<IProductDbContext>();

            ProductEntity productEntity = null;
            switch (@event)
            {
                case ProductCreatedEvent productCreatedEvent:
                    productEntity = new ProductEntity
                    {
                        Id = productCreatedEvent.Id,
                        Name = productCreatedEvent.Name,
                        Price = productCreatedEvent.Price,
                        Stock = productCreatedEvent.Stock,
                        UserId = productCreatedEvent.UserId
                    };
                    context.Products.Add(productEntity);
                    break;
                case ProductNameChangedEvent productNameChangedEvent:
                    productEntity = await context.Products.Where(w => w.Id == productNameChangedEvent.Id).FirstOrDefaultAsync();
                    if (productEntity != null)
                    {
                        productEntity.Name = productNameChangedEvent.ChangedName;
                    }
                    break;
                case ProductPriceChangedEvent productPriceChangedEvent:
                    productEntity = await context.Products.Where(w => w.Id == productPriceChangedEvent.Id).FirstOrDefaultAsync();
                    if (productEntity != null)
                    {
                        productEntity.Price = productPriceChangedEvent.ChangedPrice;
                    }
                    break;
                case ProductDeletedEvent productDeletedEvent:
                    productEntity = await context.Products.Where(w => w.Id == productDeletedEvent.Id).FirstOrDefaultAsync();
                    if (productEntity != null)
                    {
                        context.Products.Remove(productEntity);
                    }
                    break;
            }

            await context.SaveChangesAsync(CancellationToken.None);

            // autoAck true ise Exception fırlamadı ise event gönderildi sayar
            // autoAck false ise işlemi manual yapmak. eventstore'a başrılı işlem bilgisi geçmek.
            // Bu sayede event'tı tekrar fırlatıp fırlatmama kararı veriyor.
            eventStorePersistentSubscriptionBase.Acknowledge(resolvedEvent.Event.EventId);
        }
    }
}
