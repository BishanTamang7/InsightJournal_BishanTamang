using Microsoft.EntityFrameworkCore;
using InsightJournal.Models;

namespace InsightJournal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Add this DbSet for JournalEntry table
        public DbSet<JournalEntry> JournalEntries { get; set; }

        // Configure the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Make sure only one entry per day (unique constraint on Date)
            modelBuilder.Entity<JournalEntry>()
                .HasIndex(e => e.Date)
                .IsUnique();
        }
    }
}