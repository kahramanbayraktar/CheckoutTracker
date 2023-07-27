using MediatR;

namespace Checkout.API.Features.Orders.Checkouts.Commands.CashRegisterCheckout
{
    public class CashRegisterCheckoutCommand : IRequest
    {
        public string CustomerFirstName { get; set; } = null!;
        public string CustomerLastName { get; set; } = null!;

        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }

        // TODO: Add products
    }
}
