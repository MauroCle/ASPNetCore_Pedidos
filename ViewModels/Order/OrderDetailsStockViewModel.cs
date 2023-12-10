using System.ComponentModel.DataAnnotations;
using Examenes.Models;

namespace Examenes.ViewModels;

public class OrderDetailsStockViewModel{

    public int Id { get; set; }
    [Display(Name="Movimientos")]
   public ICollection<StockMovement>? StockMovements { get; set; } //Lo cambié a Icollection para probar algo, era List

   
}

