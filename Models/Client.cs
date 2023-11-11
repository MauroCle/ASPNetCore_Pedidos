namespace Examenes.Models;

public class Client
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
   // Uno a muchos (1:1)
    public Address Address { get; set; }
    // Uno a muchos (1:N)
    public ICollection<Order> Orders { get; set; }
}