using System.ComponentModel.DataAnnotations;

namespace Examenes.Models;

public class Client
{
    public int Id { get; set; }
    [Display(Name="Nombre")]
    public string FirstName { get; set; }
    [Display(Name="Apellido")]
    public string LastName { get; set; }
    [Display(Name="Email")]
    public string Email { get; set; }
    [Display(Name="Telefono")]
    public string PhoneNumber { get; set; }
   // Uno a uno (1:1)
   [Display(Name="Dirección")]
    public Address Address { get; set; }
    // Uno a muchos (1:N)
    [Display(Name="Ordenes")]
    public ICollection<Order> Orders { get; set; }
}