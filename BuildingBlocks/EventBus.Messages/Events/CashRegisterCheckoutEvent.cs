namespace EventBus.Messages.Events
{
    public class CashRegisterCheckoutEvent
    {
        // Customer
        public string CustomerFirstName { get; set; } = null!;
        public string CustomerLastName { get; set; } = null!;


        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }

        // TODO: Add products
    }
}