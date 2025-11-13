using Microsoft.EntityFrameworkCore;
using property.Domain.Entities;

namespace property.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Propierty> Propierties { get; set; }
    public DbSet<User> Users { get; set; }
}