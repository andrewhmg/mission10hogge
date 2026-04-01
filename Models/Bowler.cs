namespace Mission10Hogge.Models;

public class Bowler
{
    public int BowlerID { get; set; }
    public string? BowlerLastName { get; set; }
    public string? BowlerFirstName { get; set; }
    public string? BowlerMiddleInit { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? PhoneNumber { get; set; }
    public int TeamID { get; set; }

    public Team Team { get; set; } = null!;
}
