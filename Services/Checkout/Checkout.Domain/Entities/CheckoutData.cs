namespace Checkout.Domain.Entities
{
    public record CheckoutData
    {
        // Customer
        public string CustomerFirstName { get; set; } = null!;
        public string CustomerLastName { get; set; } = null!;


        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }

        // TODO: Add products
    }
}
