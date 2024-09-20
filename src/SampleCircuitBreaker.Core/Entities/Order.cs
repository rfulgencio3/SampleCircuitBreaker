using System.Text;

namespace SampleCircuitBreaker.Core.Entities
{
    public class Order : Base
    {
        public Order(Guid id, User user, IEnumerable<Product> products)
        {
            Id = id;
            User = user;
            Products = products;
            CreatedAt = DateTime.UtcNow;
        }

        public User User { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public override string ToString()
        {
            var productDetails = new StringBuilder();

            foreach (var product in Products)
            {
                productDetails.AppendLine($"Product: {product.Description}, Price: {product.Price:C}, Quantity: {product.Quantity}");
            }

            return @$"Order Number: {Id}, User: {User.Name}, CreatedAt: {CreatedAt:yyyy-MM-dd:hh:mm}
                   Products:\n{productDetails.ToString()}";
        }
    }
}
