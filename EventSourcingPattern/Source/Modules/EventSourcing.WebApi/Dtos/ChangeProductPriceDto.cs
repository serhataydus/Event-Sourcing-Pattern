namespace EventSourcing.WebApi.Dtos
{
    public class ChangeProductPriceDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
    }
}