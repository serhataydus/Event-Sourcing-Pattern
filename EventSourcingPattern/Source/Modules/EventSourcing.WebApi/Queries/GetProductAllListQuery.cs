using EventSourcing.WebApi.Dtos;
using MediatR;

namespace EventSourcing.WebApi.Queries
{
    public class GetProductAllListQuery:IRequest<List<ProductDto>>
    {
        public int UserId { get; set; }
    }
}
