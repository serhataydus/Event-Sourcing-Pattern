using EventSourcing.WebApi.Data;
using EventSourcing.WebApi.Dtos;
using EventSourcing.WebApi.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.WebApi.Handlers
{
    public class GetProductAllListQueryHandler : IRequestHandler<GetProductAllListQuery, List<ProductDto>>
    {
        private readonly IProductDbContext _productDbContext;

        public GetProductAllListQueryHandler(IProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<List<ProductDto>> Handle(GetProductAllListQuery request, CancellationToken cancellationToken)
        {
            var products = await _productDbContext.Products.Where(w => w.UserId == request.UserId).ToListAsync();

            return products.Select(s => new ProductDto
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                Stock = s.Stock,
                UserId = s.UserId
            }).ToList();
        }
    }
}
