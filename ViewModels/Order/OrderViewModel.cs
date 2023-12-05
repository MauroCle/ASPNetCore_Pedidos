using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class OrderViewModel{
    public int Id { get; set; }
    [Display(Name="Fecha de la orden")]
    public DateTime OrderDate { get; set; }

    [Display(Name="Dirección")]
    public string ShippingAddressData { get; set; }

    [Display(Name="Cantidad de productos")]
    public int ProductsQuantity { get; set; }

    [Display(Name="Cliente")]
    public string ClientData { get; set; }
}