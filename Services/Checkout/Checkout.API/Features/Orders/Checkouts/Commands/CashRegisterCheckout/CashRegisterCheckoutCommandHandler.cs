using EventBus.Contracts;
using MediatR;

namespace Checkout.API.Features.Orders.Checkouts.Commands.CashRegisterCheckout
{
    public class CashRegisterCheckoutCommandHandler : IRequestHandler<CashRegisterCheckoutCommand>
    {
        private readonly ILogger _logger;
        private readonly IEventBus _eventBus;

        public CashRegisterCheckoutCommandHandler(ILogger<CashRegisterCheckoutCommandHandler> logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        public async Task<Unit> Handle(CashRegisterCheckoutCommand request, CancellationToken cancellationToken)
        {
            // TODO: Usually a map is required here
            _eventBus.Publish(request);

            _logger.LogInformation("Checkout published to eventbus.");

            return Unit.Value;
        }
    }
}
