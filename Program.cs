using Microsoft.EntityFrameworkCore;
using Mission10Hogge.Data;
using Mission10Hogge.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BowlingLeagueContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BowlingLeagueDb")));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Ensure Data directory and database exist; create and seed if needed
var dataDir = Path.Combine(app.Environment.ContentRootPath, "Data");
if (!Directory.Exists(dataDir))
    Directory.CreateDirectory(dataDir);
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BowlingLeagueContext>();
    db.Database.EnsureCreated();
    if (!db.Teams.Any())
    {
        db.Teams.AddRange(
            new Team { TeamID = 1, TeamName = "Marlins" },
            new Team { TeamID = 2, TeamName = "Sharks" }
        );
        db.Bowlers.AddRange(
            new Bowler { BowlerFirstName = "Barbara", BowlerMiddleInit = null, BowlerLastName = "Fournier", TeamID = 1, Address = "457 2nd St", City = "New York", State = "NY", Zip = "10001", PhoneNumber = "(555) 111-2222" },
            new Bowler { BowlerFirstName = "David", BowlerMiddleInit = null, BowlerLastName = "Fournier", TeamID = 1, Address = "457 2nd St", City = "New York", State = "NY", Zip = "10001", PhoneNumber = "(555) 111-2222" },
            new Bowler { BowlerFirstName = "John", BowlerMiddleInit = "M", BowlerLastName = "Smith", TeamID = 2, Address = "123 Main St", City = "Provo", State = "UT", Zip = "84601", PhoneNumber = "(555) 333-4444" }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.UseCors();

app.MapGet("/api/bowlers", async (BowlingLeagueContext db) =>
{
    var bowlers = await db.Bowlers
        .Include(b => b.Team)
        .Where(b => b.Team.TeamName == "Marlins" || b.Team.TeamName == "Sharks")
        .OrderBy(b => b.Team.TeamName)
        .ThenBy(b => b.BowlerLastName)
        .Select(b => new BowlerDto
        {
            BowlerName = $"{b.BowlerFirstName} {b.BowlerMiddleInit} {b.BowlerLastName}".Trim().Replace("  ", " "),
            TeamName = b.Team.TeamName,
            Address = b.Address ?? "",
            City = b.City ?? "",
            State = b.State ?? "",
            Zip = b.Zip ?? "",
            PhoneNumber = b.PhoneNumber ?? ""
        })
        .ToListAsync();
    return Results.Ok(bowlers);
})
.WithName("GetBowlers")
.WithOpenApi();

app.Run();