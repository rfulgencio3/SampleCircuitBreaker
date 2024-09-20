using Microsoft.OpenApi.Models;
using SampleCircuitBreaker.API.Settings;
using SampleCircuitBreaker.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.AddHttpClient<IMessagePublisherService, MessagePublisherService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SampleCircuitBreaker API", Version = "v1" });
});

var app = builder.Build();

app.UseStaticFiles();

app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleCircuitBreaker API v1");
    c.RoutePrefix = "swagger"; 
});

Console.WriteLine($"Current Environment: {builder.Environment.EnvironmentName}");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
