using CheckoutDataParser.Entities;
using EventBus.Messages.Events;

namespace DataParser.Parser
{
    public static class MessageParser
    {
        public static CustomerData Parse(CheckoutData message)
        {
            CustomerData customerInfo = new()
            {
                FirstName = message.CustomerFirstName,
                LastName = message.CustomerLastName
            };

            return customerInfo;
        }
    }
}
