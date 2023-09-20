using RabbitMQ.Client.Events;

namespace OnEntitySharedLogic.RabbitMq;

public interface IMessageConsumer
{
    void ConsumeMessage(string queueName, Action<BasicDeliverEventArgs> messageHandler);
}