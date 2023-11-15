using System.ComponentModel.DataAnnotations;

namespace Examenes.Models;

public class Product
{
    public int Id { get; set; }
    [Display(Name="Nombre")]
    public string Name { get; set; }
    [Display(Name="Descripción")]
    public string Description { get; set; }
    [Display(Name="Precio")]
    public int Price { get; set; }
    // Muchos a muchos (N:N)
    [Display(Name="Ordenes")]
    public ICollection<Order> Orders { get; set; }
}