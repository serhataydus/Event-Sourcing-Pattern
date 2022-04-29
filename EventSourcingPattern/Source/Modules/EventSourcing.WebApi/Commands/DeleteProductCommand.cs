using MediatR;

namespace EventSourcing.WebApi.Commands
{
    public class DeleteProductCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
