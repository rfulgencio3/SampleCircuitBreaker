public interface IMessagePublisherService
{
    Task PublishAsync(string destination, string message);
}
