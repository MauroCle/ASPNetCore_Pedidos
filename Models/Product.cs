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
    public float Price { get; set; }
     [Display(Name="Precio")]
    public int Stock {get;set;}
    // Muchos a muchos (N:N)
    [Display(Name="Ordenes")]
    public ICollection<Order>? Orders { get; set; }
    //Muchos a uno (N:1)
    public ICollection<StockMovement>? StockMovements { get; set; }
}