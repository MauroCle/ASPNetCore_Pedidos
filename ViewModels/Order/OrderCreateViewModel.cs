using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class OrderCreateViewModel{

   public DateTime OrderDate {get; set;}
   public int ClientId {get; set;}
   public List<int> ProductIds {get; set;}
   public List<Client> Clients { get; set; }
   public List<Product> Products { get; set; }
}

