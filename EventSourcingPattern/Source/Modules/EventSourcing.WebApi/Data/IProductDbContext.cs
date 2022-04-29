using EventSourcing.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.WebApi.Data
{
    public interface IProductDbContext
    {
        DbSet<ProductEntity> Products { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
