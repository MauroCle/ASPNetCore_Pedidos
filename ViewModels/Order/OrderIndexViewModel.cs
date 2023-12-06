using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class OrderIndexViewModel{

    [Display(Name="Pedido")]
    public List<OrderViewModel> Orders {get; set;} = new List<OrderViewModel>();
    [Display(Name="Filter")]
    public string? Filter{get; set;}

}