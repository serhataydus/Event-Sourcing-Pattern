using EventSourcing.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.WebApi.Data
{
    public class ProductDbContext : DbContext, IProductDbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<ProductEntity> Products { get; set; }
    }
}
