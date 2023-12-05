using Examenes.Models;

namespace Examenes.ViewModels;

public class AddressDeleteViewModel{

    public int Id {get;set;}
    public string City {get; set;} = null!;
    public string Street {get; set;} = null!;
    public int Number {get; set;}
    public string Apartment {get; set;} = null!;
    public string? Notes {get; set;}
    public string PostalCode {get; set;} = null!;
    public string ClientData {get;set;} = null!;

}