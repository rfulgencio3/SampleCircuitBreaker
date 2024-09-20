using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using SampleCircuitBreaker.Application.Constants;
using System.Text;

namespace SampleCircuitBreaker.Application.Services;

public class MessagePublisherService : IMessagePublisherService
{
    private readonly IConfiguration _configuration;

    public MessagePublisherService(IConfiguration configuration)
    {
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
            string retryQueueName = MessageBrokerConstants.ORDER_QUEUE_NAME_RETRY;
            string retryExchangeName = MessageBrokerConstants.ORDER_QUEUE_NAME_RETRY;
            string deadLetterQueueName = MessageBrokerConstants.ORDER_QUEUE_NAME_DLQ;
            string deadLetterExchangeName = MessageBrokerConstants.ORDER_QUEUE_NAME_DLQ;

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
            channel.ExchangeDeclare(exchange: retryExchangeName, type: ExchangeType.Direct);
            channel.ExchangeDeclare(exchange: deadLetterExchangeName, type: ExchangeType.Direct);

            channel.QueueDeclare(queue: deadLetterQueueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.QueueDeclare(queue: retryQueueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: new Dictionary<string, object>
                                 {
                                     { "x-message-ttl", 5000 },
                                     { "x-dead-letter-exchange", exchangeName },
                                     { "x-dead-letter-routing-key", routingKey }
                                 });

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: new Dictionary<string, object>
                                 {
                                     { "x-dead-letter-exchange", deadLetterExchangeName },
                                     { "x-dead-letter-routing-key", routingKey }
                                 });

            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
            channel.QueueBind(queue: retryQueueName, exchange: retryExchangeName, routingKey: routingKey);
            channel.QueueBind(queue: deadLetterQueueName, exchange: deadLetterExchangeName, routingKey: routingKey);

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
