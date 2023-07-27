using Checkout.Domain.Entities;
using Checkout.API.EventBus;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;

        public CheckoutController(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public IActionResult Checkout([FromBody] CheckoutData checkout)
        {
            _messageProducer.Publish(checkout);

            return Accepted();
        }
    }
}
