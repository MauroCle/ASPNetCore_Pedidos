using Examenes.Models;

namespace Examenes.ViewModels;

public class AddressViewModel{

    public int Id {get;set;}
    public string City {get; set;} = null!;
    public string Street {get; set;} = null!;
    public int Number {get; set;}
    public string Apartment {get; set;} = null!;
    public string? Notes {get; set;}
    public string PostalCode {get; set;} = null!;
    public string ClientFirstName {get;set;} = null!;
    public string ClientLastName {get;set;} =null!;
}