using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class AddressDeleteViewModel{

    public int Id {get;set;}
    [Display(Name = "Ciudad")]
    public string City { get; set; } = null!;
    [Display(Name = "Calle")]
    public string Street { get; set; } = null!;
    [Display(Name = "Numero")]
    public int Number { get; set; }
    [Display(Name = "Departamento")]
    public string Apartment { get; set; } = null!;
    [Display(Name = "Notas")]
    public string? Notes { get; set; }
    [Display(Name = "Codigo postal")]
    public string PostalCode { get; set; } = null!;
    [Display(Name = "Cliente")]
    public string ClientData {get;set;} = null!;

}