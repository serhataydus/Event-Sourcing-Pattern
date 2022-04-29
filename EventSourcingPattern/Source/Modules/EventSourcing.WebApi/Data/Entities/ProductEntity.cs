using Microsoft.EntityFrameworkCore;

namespace EventSourcing.WebApi.Data.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }


        public int Stock { get; set; }
        public int UserId { get; set; }
    }
}
