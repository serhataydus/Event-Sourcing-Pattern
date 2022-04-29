using EventSource.Shared.Events.Classes;
using EventSourcing.WebApi.Dtos;
using EventStore.ClientAPI;

namespace EventSourcing.WebApi.EventStores
{
    public class ProductStream : AbstractStream
    {
        public const string ProductName = "ProductStream";
        public const string SreamGroupName = "agroup";
        //public const string SreamGroupName = "replay"; // DB uçarsa tekrar ilk eventtan itibaren re-play yapılıyor. Yeni bir Subscriptions üzerinden.

        public ProductStream(IEventStoreConnection eventStoreConnection) : base(ProductName, eventStoreConnection)
        {
        }

        public void Created(CreateProductDto createProductDto)
        {
            Events.AddLast(new ProductCreatedEvent
            {
                Id = Guid.NewGuid(),
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                UserId = createProductDto.UserId
            });
        }

        public void NameChanged(ChangeProductNameDto changeProductNameDto)
        {
            Events.AddLast(new ProductNameChangedEvent
            {
                Id = changeProductNameDto.Id,
                ChangedName = changeProductNameDto.Name
            });
        }

        public void PriceChanged(ChangeProductPriceDto changeProductPriceDto)
        {
            Events.AddLast(new ProductPriceChangedEvent
            {
                Id = changeProductPriceDto.Id,
                ChangedPrice = changeProductPriceDto.Price
            });
        }

        public void Deleted(Guid id)
        {
            Events.AddLast(new ProductDeletedEvent
            {
                Id = id
            });
        }
    }
}
