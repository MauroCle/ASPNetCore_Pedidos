using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class OrderDetailViewModel{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public string ShippingAddressData { get; set; }

    public List<Product> Products {get;set;}

    public string ClientData { get; set; }
}