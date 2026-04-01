namespace Mission10Hogge.Models;

public class Team
{
    public int TeamID { get; set; }
    public string TeamName { get; set; } = string.Empty;

    public ICollection<Bowler> Bowlers { get; set; } = new List<Bowler>();
}
