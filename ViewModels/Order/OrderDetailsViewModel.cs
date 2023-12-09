using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class OrderDetailViewModel{
    public int Id { get; set; }
    [Display(Name="Fecha de la orden")]
    public DateTime OrderDate { get; set; }
    [Display(Name="Direccion de entrega")]
    public string ShippingAddressData { get; set; }
    [Display(Name="Productos")]
    public List<Product> Products {get;set;}
    [Display(Name="Datos del cliente")]
    public string ClientData { get; set; }
    [Display(Name="Stock Disponible")]
    public Dictionary<int, int> ProductStockDictionary { get; set; }

}