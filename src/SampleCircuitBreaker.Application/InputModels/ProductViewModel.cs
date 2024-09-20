namespace SampleCircuitBreaker.Application.ViewModels;

public class ProductsViewModel
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
}