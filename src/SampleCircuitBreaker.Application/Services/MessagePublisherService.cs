using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using SampleCircuitBreaker.Application.Constants;
using System.Text;

namespace SampleCircuitBreaker.Application.Services;

public class MessagePublisherService : IMessagePublisherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public MessagePublisherService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task PublishAsync(string destination, string message)
    {
        var rabbitMqSettings = _configuration.GetSection("RabbitMqSettings");

        var factory = new ConnectionFactory()
        {
            HostName = rabbitMqSettings["HostName"],
            UserName = rabbitMqSettings["UserName"],
            Password = rabbitMqSettings["Password"]
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            string exchangeName = MessageBrokerConstants.ORDER_EXCHANGE_NAME;
            string queueName = MessageBrokerConstants.ORDER_QUEUE_NAME;
            string routingKey = MessageBrokerConstants.ORDER_ROUTING_KEY;

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: routingKey);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($" [x] Sent {message}");
        }

        await Task.CompletedTask;
    }
}
