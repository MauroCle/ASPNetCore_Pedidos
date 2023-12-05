using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class ProductIndexViewModel{

    public List<ProductViewModel> Products {get; set;} = new List<ProductViewModel>();
    public string? NameFilter{get; set;}

}
