using MagicVilla.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Villa> Villas { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
}