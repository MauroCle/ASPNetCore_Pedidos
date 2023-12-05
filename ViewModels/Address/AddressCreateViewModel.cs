using Examenes.Models;

namespace Examenes.ViewModels;

public class AddressCreateViewModel{

    public string City {get; set;} = null!;
    public string Street {get; set;} = null!;
    public int Number {get; set;}
    public string Apartment {get; set;} = null!;
    public string? Notes {get; set;}
    public string PostalCode {get; set;} = null!;
    public int ClientId {get;set;}
public List<Client> Clients { get; set; } = new List<Client>();
}

