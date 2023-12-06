using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class ProductViewModel{

    public int Id { get; set; }
    [Display(Name="Nombre")]
    public string Name { get; set; }
    [Display(Name="Descripción")]
    public string Description { get; set; }
    [Display(Name="Precio")]
    public float Price { get; set; }
    [Display(Name="Stock")]
    public int Stock { get; set; }
}

