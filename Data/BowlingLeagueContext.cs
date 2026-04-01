using Microsoft.EntityFrameworkCore;
using Mission10Hogge.Models;

namespace Mission10Hogge.Data;

public class BowlingLeagueContext : DbContext
{
    public BowlingLeagueContext(DbContextOptions<BowlingLeagueContext> options)
        : base(options) { }

    public DbSet<Bowler> Bowlers { get; set; }
    public DbSet<Team> Teams { get; set; }
}
