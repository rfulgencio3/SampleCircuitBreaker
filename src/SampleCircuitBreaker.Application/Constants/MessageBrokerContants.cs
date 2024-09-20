namespace SampleCircuitBreaker.Application.Constants;

public static class MessageBrokerConstants
{
    public const string ORDER_EXCHANGE_NAME = "order_exchange";
    public const string ORDER_QUEUE_NAME = "order_queue";
    public const string ORDER_ROUTING_KEY = "order.created";
}
