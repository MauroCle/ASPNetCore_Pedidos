using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class OrderCreateViewModel{

   [Display(Name="Fecha de la orden")]
   public DateTime OrderDate {get; set;}
   [Display(Name="Cliente")]
   public int ClientId {get; set;}
   [Display(Name="Productos seleccionados")]
   public List<int> ProductIds {get; set;}
   [Display(Name = "Stock de productos")]
   public Dictionary<int, int> ProductStockDictionary { get; set; }
   public List<Client> Clients { get; set; }
    [Display(Name="Productos")]
   public List<Product> Products { get; set; }
}

