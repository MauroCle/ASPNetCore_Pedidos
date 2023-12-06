using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class ProductIndexViewModel{
    [Display(Name="Productos")]
    public List<ProductViewModel> Products {get; set;} = new List<ProductViewModel>();
    [Display(Name="Filtro")]
    public string? NameFilter{get; set;}

}
