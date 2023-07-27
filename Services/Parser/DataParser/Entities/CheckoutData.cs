namespace EventBus.Messages.Events
{
    public class CheckoutData
    {
        // Customer
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }


        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }

        // TODO: Add products
    }
}