using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class ClientViewModel{
    [Display(Name="Clientes")]
    public List<Client> Clients {get; set;} = new List<Client>();
    [Display(Name="Filter")]
    public string? NameFilter{get; set;}


}
