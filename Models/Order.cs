namespace Examenes.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    // Muchos a uno (N:1)
    public int AddressId { get; set; }
    public Address ShippingAddress { get; set; }
    //Muchos a muchos (N:N)
    public ICollection<Product> Products { get; set; }
    // Uno a muchos (1:N)
    public int ClientId { get; set; }
    public Client Client { get; set; }
}