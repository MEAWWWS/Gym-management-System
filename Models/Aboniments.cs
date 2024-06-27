namespace Gym.Models;

public partial class Aboniments
{
    public int AbonimentId { get; set; }

    public DateOnly PurchaseDate { get; set; }

    public DateOnly DeadlineDate { get; set; }

    public decimal Price { get; set; }

    //in sql script set that fk as pk
    public virtual ICollection<Clients> Clients { get; set; } = new List<Clients>();
}
