using System.ComponentModel.DataAnnotations;

namespace Examenes.Models;

public class Order
{
    public int Id { get; set; }
    [Display(Name="Fecha de la orden")]
    public DateTime OrderDate { get; set; }
    // Muchos a uno (N:1)
    public int AddressId { get; set; }
    [Display(Name="Dirección")]
    public Address ShippingAddress { get; set; }
    //Muchos a muchos (N:N)
    [Display(Name="Productos")]
    public ICollection<Product> Products { get; set; }
    // Uno a muchos (1:N)
    public int ClientId { get; set; }
    [Display(Name="Cliente")]
    public Client Client { get; set; }
}