using Microsoft.AspNetCore.Mvc;
using SampleCircuitBreaker.Application.InputModels;
using System.Text.Json;

namespace SampleCircuitBreaker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessagePublisherService _messagePublisherService;

    public MessageController(IMessagePublisherService messagePublisherService)
    {
        _messagePublisherService = messagePublisherService;
    }

    // POST api/message
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateOrderInputModel inputModel)
    {
        var message = JsonSerializer.Serialize(inputModel);
        await _messagePublisherService.PublishAsync("defaultQueue", message);

        return Ok(new { Status = "Message published successfully" });
    }
}
