namespace Checkout.API.EventBus
{
    public interface IMessageProducer
    {
        void Publish<T>(T message);
    }
}
