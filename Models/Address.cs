using System.ComponentModel.DataAnnotations;

namespace Examenes.Models;

public class Address
{
    public int Id { get; set; }
    [Display(Name="Ciudad")]
    public string City { get; set; }
    [Display(Name="Calle")]
    public string Street { get; set; }
    [Display(Name="Numero")]
    public int Number { get; set; }
    [Display(Name="Departamento")]
    public string Apartment{ get; set; }
    [Display(Name="Notas")]
    public string Notes { get; set; }
    [Display(Name="Codigo Postal")]
    public string PostalCode { get; set; }
    [Display(Name="ClienteId")]
    // uno a uno (1:1)
    public int ClientId { get; set; }
    [Display(Name="Cliente")]
    public Client Client { get; set; }
    [Display(Name="Ordenes")]
    //uno a muchos (1:N)
    public ICollection<Order> Orders { get; set; }
}