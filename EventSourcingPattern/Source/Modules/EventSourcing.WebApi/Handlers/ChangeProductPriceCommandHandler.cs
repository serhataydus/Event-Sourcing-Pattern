using EventSourcing.WebApi.Commands;
using EventSourcing.WebApi.EventStores;
using MediatR;

namespace EventSourcing.WebApi.Handlers
{
    public class ChangeProductPriceCommandHandler : IRequestHandler<ChangeProductPriceCommand>
    {
        private readonly ProductStream _productStream;

        public ChangeProductPriceCommandHandler(ProductStream productStream)
        {
            _productStream = productStream;
        }

        public async Task<Unit> Handle(ChangeProductPriceCommand request, CancellationToken cancellationToken)
        {
            _productStream.PriceChanged(request.ChangeProductPriceDto);

            await _productStream.SaveAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
