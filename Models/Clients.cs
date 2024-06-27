namespace Gym.Models;

public partial class Clients
{
    public int ClientId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public int AbonimentId { get; set; }

    public int TrainerId { get; set; }

    public virtual Aboniments Aboniment { get; set; } = null!;

    public virtual ICollection<Sessions> Sessions { get; set; } = new List<Sessions>();

    public virtual TrainersInfo Trainer { get; set; } = null!;
}
