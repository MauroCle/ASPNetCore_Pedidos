using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class OrderEditViewModel{

    public int Id { get; set; }
     [Display(Name="Fecha de la orden")]
    public DateTime OrderDate { get; set; }
     [Display(Name="Productos seleccionados")]
   public List<int> ProductIds {get; set;}
    [Display(Name="Cliente")]
    public int ClientId { get; set; }
     [Display(Name="Clientes")]
   public List<Client> Clients { get; set; }
    [Display(Name="Productos")]
   public List<Product> Products { get; set; }
   
}

