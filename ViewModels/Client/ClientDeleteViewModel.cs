using Examenes.Models;

namespace Examenes.ViewModels;

public class ClientDeleteViewModel{

    public int Id {get; set;}
    public string FirstName {get; set;} = null!;
    public string LastName {get; set;} = null!;
    public string Email {get; set;} = null!;
   public string PhoneNumber {get; set;} = null!;
    public List<Address> Addresses {get; set;} = new List<Address>(); //TODO esto podria ser una lista de AddressDetailViewModel

}