using Checkout.API.Features.Orders.Checkouts.Commands.CashRegisterCheckout;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public CheckoutController(ILogger<CheckoutController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CashRegisterCheckoutCommand command)
        {
            await _mediator.Send(command);

            _logger.LogInformation("Checkout received.");

            return Accepted();
        }
    }
}
