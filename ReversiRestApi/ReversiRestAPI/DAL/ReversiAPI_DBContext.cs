using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ReversieISpelImplementatie.Model;

public class ReversiAPI_DBContext : DbContext
{
    public ReversiAPI_DBContext(DbContextOptions<ReversiAPI_DBContext> options) : base(options)
    {
    }

    public DbSet<Spel> Spellen { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ? 
    }
}