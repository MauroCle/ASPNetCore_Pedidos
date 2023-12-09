using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class OrderViewModel{
    public int Id { get; set; }
    [Display(Name="Fecha de la orden")]
    public DateTime OrderDate { get; set; }

    [Display(Name="Dirección")]
    public string ShippingAddressData { get; set; }

    [Display(Name="Variedad de productos")]
    public int ProductsQuantity { get; set; } //TODO cambiar el nombre de la variable donde se use. Ya no es representativo.

    [Display(Name="Cliente")]
    public string ClientData { get; set; }
}