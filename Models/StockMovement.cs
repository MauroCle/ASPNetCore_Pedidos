using Examenes.Models;

namespace Examenes.Models;
public class StockMovement
{
    public int Id { get; set; }
    public int IdOrder { get; set; }
    public int IdProduct { get; set; }
    public MovementType MovementType { get; set; }
    public int Quantity { get; set; }

    //Uno a muchos (1:N)
    public virtual Order Order { get; set; }
    //Uno a muchos (1:N)
    public virtual Product Product { get; set; }
}

public enum MovementType
{
    In,
    Out
}