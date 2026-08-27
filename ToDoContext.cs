using Microsoft.EntityFrameworkCore;
using ToDoListConsole.Models;

namespace ToDoListConsole.Data
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
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
        }
    }
}
