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
   public ICollection<Client>? Clients { get; set; } //Lo cambié a Icollection para probar algo, era List
    [Display(Name="Productos")]
   public ICollection<Product>? Products { get; set; } //Lo cambié a Icollection para probar algo, era List
    [Display(Name="Stock Disponible")]
    public Dictionary<int, int> ProductStockDictionary { get; set; }
   
}

