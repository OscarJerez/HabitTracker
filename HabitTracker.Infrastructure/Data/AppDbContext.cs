using HabitTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Habit> Habits => Set<Habit>();
    public DbSet<HabitUser> HabitUsers => Set<HabitUser>(); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

 
        modelBuilder.Entity<HabitUser>().ToTable("Users");

        // Hide soft-deleted habits from all normal queries
        modelBuilder.Entity<Habit>()
             .HasQueryFilter(habit => !habit.IsDeleted);
    }
}
