using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace OnEntitySharedLogic.RabbitMq;

public class MessagePublisher : IMessagePublisher
{
    private readonly IModel _channel;
    private readonly ILogger<MessagePublisher> _logger;
    private readonly string _host = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker" ? "rabbitmq" : "localhost";

    public MessagePublisher(ILogger<MessagePublisher> logger)
    {
        var factory = new ConnectionFactory
        {
            HostName = _host
        };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        
        _logger = logger;
    }

    public void PublishMessage<TType>(TType message, string routingKey)
    {
        var messageToJson = JsonConvert.SerializeObject(message);
        var messageToByteArray = Encoding.UTF8.GetBytes(messageToJson);

        try
        {
            _channel.BasicPublish(exchange: "", routingKey: routingKey, body: messageToByteArray);
            _logger.LogInformation($"{DateTime.Now} ---> Message published!");
        }
        catch (Exception exception)
        {
            _logger.LogError($"{DateTime.Now} ---> {exception}");
            throw;
        }
    }
}