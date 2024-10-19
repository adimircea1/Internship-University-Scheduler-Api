namespace OnEntitySharedLogic.RabbitMq;

public interface IMessagePublisher
{
    public void PublishMessage<TType>(TType message, string routingKey);
}