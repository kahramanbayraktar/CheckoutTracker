namespace EventBus.Contracts;

public interface IEventBus
{
    void Publish<T>(T message);
    void Consume(Action<string> messageHandler);
}
