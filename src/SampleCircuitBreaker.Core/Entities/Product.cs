namespace SampleCircuitBreaker.Core.Entities;

public class Product : Base
{
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}