using System.ComponentModel.DataAnnotations;

namespace Examenes.ViewModels;
public class AddressIndexViewModel{
    [Display(Name="Direcciones")]
    public List<AddressViewModel> Addresses {get; set;} = new List<AddressViewModel>();
    [Display(Name="Filter")]
    public string? Filter{get; set;}
}