using Checkout.API.EventBus;
using MediatR;

namespace Checkout.API.Features.Orders.Checkouts.Commands.CashRegisterCheckout
{
    public class CashRegisterCheckoutCommandHandler : IRequestHandler<CashRegisterCheckoutCommand>
    {
        private readonly IMessageProducer _messageProducer;

        public CashRegisterCheckoutCommandHandler(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        public async Task<Unit> Handle(CashRegisterCheckoutCommand request, CancellationToken cancellationToken)
        {
            // TODO: Usually a map is required here
            _messageProducer.Publish(request);

            return Unit.Value;
        }
    }
}
