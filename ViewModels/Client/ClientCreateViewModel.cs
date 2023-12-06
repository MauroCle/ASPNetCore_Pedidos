using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class ClientCreateViewModel{

   public int Id {get; set;}
    [Display(Name = "Nombre")]
    public string FirstName {get; set;} = null!;
    [Display(Name = "Apellido")]
    public string LastName {get; set;} = null!;
    [Display(Name = "Email")]
    public string Email {get; set;} = null!;
    [Display(Name = "Numero telefonico")]
    public string PhoneNumber {get; set;} = null!;
    [Display(Name = "Ciudad")]
    public string City {get; set;} = null!;
    [Display(Name = "Calle")]
    public string Street {get; set;} = null!;
    [Display(Name = "Numero")]
    public int Number {get; set;}
    [Display(Name = "Departamento")]
    public string Apartment {get; set;} = null!;
    [Display(Name = "Notas")]
    public string? Notes {get; set;}
    [Display(Name = "Codigo postal")]
    public string PostalCode {get; set;} = null!;
}
