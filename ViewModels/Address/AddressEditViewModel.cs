using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class AddressEditViewModel{
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
    public int ClientId {get;set;}
    [Display(Name = "Clientes")]
    public List<Client> Clients { get; set; } = new List<Client>();
}

