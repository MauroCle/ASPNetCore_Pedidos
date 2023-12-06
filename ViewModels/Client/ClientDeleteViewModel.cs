using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class ClientDeleteViewModel{

    public int Id {get; set; }
    [Display(Name = "Nombre")]
    public string FirstName {get; set;} = null!;
    [Display(Name = "Apellido")]
    public string LastName {get; set;} = null!;
    [Display(Name = "Email")]
    public string Email {get; set;} = null!;
    [Display(Name = "Numero telefonico")]
    public string PhoneNumber {get; set;} = null!;
    [Display(Name = "Direcciones")]
    public List<Address> Addresses {get; set;} = new List<Address>();

}