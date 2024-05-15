using Microsoft.EntityFrameworkCore;
using ReversiMVCApplication.Models;

namespace ReversiMVCApplication.Data;

public class ReversiDbContext : DbContext
{
    public ReversiDbContext(DbContextOptions<ReversiDbContext> options) : base(options)
    {
    }

    public DbSet<Speler> Spelers { get; set; }

    public DbSet<Spel>? Spel { get; set; }

    public DbSet<ReversiMVCApplication.Models.AdminRequest>? AdminRequest { get; set; }
}