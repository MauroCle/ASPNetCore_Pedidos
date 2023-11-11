namespace Examenes.Models;

public class Address
{
    public int Id { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }
    public string Apartment{ get; set; }
    public string Notes { get; set; }
    public string PostalCode { get; set; }
    // uno a uno (1:1)
    public int ClientId { get; set; }
    public Client Client { get; set; }
    //uno a muchos (1:N)
    public ICollection<Order> Orders { get; set; }
}