using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using SampleCircuitBreaker.Application.Services;

namespace SampleCircuitBreaker.Application.Tests.Services;

public class MessagePublisherServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly MessagePublisherService _messagePublisherService;
    private readonly Faker _faker;

    public MessagePublisherServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _faker = new Faker();

        var rabbitMqSection = new Mock<IConfigurationSection>();
        rabbitMqSection.Setup(x => x["HostName"]).Returns("localhost");
        rabbitMqSection.Setup(x => x["UserName"]).Returns("admin");
        rabbitMqSection.Setup(x => x["Password"]).Returns("Admin*123");

        _configurationMock.Setup(x => x.GetSection("RabbitMqSettings")).Returns(rabbitMqSection.Object);

        _messagePublisherService = new MessagePublisherService(new HttpClient(), _configurationMock.Object);
    }

    [Fact]
    public async Task PublishAsync_ShouldPublishMessage_WhenCircuitIsClosed()
    {
        // Arrange
        var destination = _faker.Random.Word();
        var message = _faker.Lorem.Text();

        // Act
        Func<Task> act = async () => { await _messagePublisherService.PublishAsync(destination, message); };

        // Assert
        await act.Should().NotThrowAsync();
    }
}
