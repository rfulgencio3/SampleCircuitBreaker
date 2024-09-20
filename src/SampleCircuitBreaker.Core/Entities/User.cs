namespace SampleCircuitBreaker.Core.Entities;

public class User : Base
{
    public User(string name, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        CreatedAt = DateTime.UtcNow;
    }

    public string Name { get; set; }
    public string Email { get; set; }
}
