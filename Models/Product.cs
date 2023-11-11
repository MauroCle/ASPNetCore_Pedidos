namespace Examenes.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    // Muchos a muchos (N:N)
    public ICollection<Order> Orders { get; set; }
}