namespace EventBus.Messages.EventBus
{
    public interface IEventBus
    {
        void Publish<T>(T message);
        void Consume(Action<string> messageHandler);
    }
}
