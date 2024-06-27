namespace Gym.Models;

public partial class TrainersInfo
{
    public int TrainerId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public string Schedule { get; set; } = null!;

    public virtual ICollection<Clients> Clients { get; set; } = new List<Clients>();

    public virtual ICollection<Sessions> Sessions { get; set; } = new List<Sessions>();

    public virtual Users User { get; set; } = null!;
}
