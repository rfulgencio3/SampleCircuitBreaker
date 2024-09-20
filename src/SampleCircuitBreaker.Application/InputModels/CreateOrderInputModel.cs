using SampleCircuitBreaker.Application.ViewModels;

namespace SampleCircuitBreaker.Application.InputModels;

public record CreateOrderInputModel
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required IEnumerable<ProductsViewModel> ProductsViewModel { get; set; }
}

