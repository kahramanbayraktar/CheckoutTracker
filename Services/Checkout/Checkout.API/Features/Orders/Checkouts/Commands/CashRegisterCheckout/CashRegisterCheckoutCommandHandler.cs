using EventBus.Contracts;
using MediatR;

namespace Checkout.API.Features.Orders.Checkouts.Commands.CashRegisterCheckout
{
    public class CashRegisterCheckoutCommandHandler : IRequestHandler<CashRegisterCheckoutCommand>
    {
        private readonly IEventBus _eventBus;

        public CashRegisterCheckoutCommandHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task<Unit> Handle(CashRegisterCheckoutCommand request, CancellationToken cancellationToken)
        {
            // TODO: Usually a map is required here
            _eventBus.Publish(request);

            return Unit.Value;
        }
    }
}
