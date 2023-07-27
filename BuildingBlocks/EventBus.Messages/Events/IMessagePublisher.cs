namespace EventBus.Messages.Events
{
    public interface IMessagePublisher
    {
        void Publish<T>(T message);
    }
}
