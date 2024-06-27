namespace Gym.Models;

public partial class Sessions
{
    public int SessionId { get; set; }

    public int ClientId { get; set; }

    public int TrainerId { get; set; }

    public DateTime SessionStartDateTime { get; set; }

    public DateOnly SessionDate { get; set; }

    public TimeOnly SessionTime { get; set; }

    public virtual Clients Client { get; set; } = null!;

    public virtual TrainersInfo Trainer { get; set; } = null!;
}
