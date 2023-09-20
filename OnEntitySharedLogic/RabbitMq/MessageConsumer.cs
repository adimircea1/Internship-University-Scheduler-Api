using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OnEntitySharedLogic.RabbitMq;

//In the future I will have to handle messages that fail to be delivered
public class MessageConsumer : IMessageConsumer
{
    private readonly ILogger<MessageConsumer> _logger;
    private readonly IModel _channel;
    private readonly string _host = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker" ? "rabbitmq" : "localhost";

    public MessageConsumer(ILogger<MessageConsumer> logger)
    {
        _logger = logger;
        var factory = new ConnectionFactory
        {
            HostName = _host
        };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
    }

    public void ConsumeMessage(string queueName, Action<BasicDeliverEventArgs> messageHandler)
    {
        //durable -> false => messages and the queue will be deleted if the broker stops working
        //durable -> true => messages and the queue will be saved to disk if the broker stops working
        _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments:null);

        var consumer = new EventingBasicConsumer(_channel);
        
        //A new scope is being created below
        consumer.Received += (_, eventArguments) =>
        {
            try
            {
                messageHandler(eventArguments);
                _channel.BasicAck(eventArguments.DeliveryTag, false);
            }
            catch (Exception exception)
            {
                _logger.LogError($"{DateTime.Now} ---> Error processing message - {exception}");
                _channel.BasicNack(eventArguments.DeliveryTag, false, true);
            }
        };
        
        _channel.BasicConsume(queue: queueName, consumer: consumer);
        _logger.LogInformation($"{DateTime.Now} ---> Consumer is waiting for messages...");
    }
}