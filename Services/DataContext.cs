using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoListConsole.Models;

namespace ToDoListConsole.Services;
public partial class ToDoListContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public ToDoListContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(Configuration.GetConnectionString("ToDoDatabase"));
    }
    public DbSet<ToDoRecords> SaveRecords { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ToDoRecords>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Priority).IsRequired();
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.TaskStatus).IsRequired();
            entity.Property(e => e.DateTime);
        });
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}