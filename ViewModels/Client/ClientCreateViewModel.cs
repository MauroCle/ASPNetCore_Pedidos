using Examenes.Models;

namespace Examenes.ViewModels;

public class ClientCreateViewModel{

   public int Id {get; set;}
    public string FirstName {get; set;} = null!;
    public string LastName {get; set;} = null!;
    public string Email {get; set;} = null!;
   public string PhoneNumber {get; set;} = null!;
   public string City {get; set;} = null!;
    public string Street {get; set;} = null!;
    public int Number {get; set;}
    public string Apartment {get; set;} = null!;
    public string? Notes {get; set;}
    public string PostalCode {get; set;} = null!;
}
