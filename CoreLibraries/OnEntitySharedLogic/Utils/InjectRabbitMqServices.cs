using Microsoft.Extensions.DependencyInjection;
using OnEntitySharedLogic.RabbitMq;

namespace OnEntitySharedLogic.Utils;

public static class InjectRabbitMqServices
{
    public static void InjectRabbitMq(this IServiceCollection services)
    {
        services.AddScoped<IMessagePublisher, MessagePublisher>();
        services.AddScoped<IMessageConsumer, MessageConsumer>();
    }
}