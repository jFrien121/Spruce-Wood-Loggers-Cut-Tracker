using Microsoft.EntityFrameworkCore;
using Spruce_Wood_Loggers_ERP.Database_Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;

/**
 * AppDbContext
 * Create a database context for the application
 * using Entity Framework Core.
 */

namespace Spruce_Wood_Loggers_ERP
{
    class AppDbContext : DbContext
    {
        public DbSet<Batch> Batches { get; set; }
        public DbSet<CutLength> CutLengths { get; set; }
        public DbSet<CutSize> CutSizes { get; set; }
        public DbSet<StandardNumPieces> StandardNumPieces { get; set; }
        public DbSet<StandardSizeRelationship> StandardSizeRelationships { get; set; }
        public DbSet<LiftResult> LiftResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            try
            {
                string json = File.ReadAllText(DatabaseConfig.getConfigPath());

                DatabaseConfig dbConfig = JsonSerializer.Deserialize<DatabaseConfig>(json)!;
                options.UseNpgsql($"Host={dbConfig.ipAddress};Port={dbConfig.port};Database=Cut_Tracker_Database;" +
                    $"Username={dbConfig.username};Password={dbConfig.password}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error configuring database: {ex.Message}\n\nApplication may need to be restarted.",
                    "Database Configuration Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {

                modelBuilder.Entity<Batch>()
                    .Property(x => x.timeProcessed)
                    .HasColumnType("timestamp without time zone");

                // Ensure uniqueness for certain fields
                modelBuilder.Entity<CutLength>()
                    .HasIndex(l => l.length)
                    .IsUnique();

                modelBuilder.Entity<CutSize>()
                    .HasIndex(s => new { s.thickness, s.width })
                    .IsUnique();

                modelBuilder.Entity<StandardNumPieces>()
                    .HasIndex(s => s.numPieces)
                    .IsUnique();

                modelBuilder.Entity<StandardSizeRelationship>()
                    .HasIndex(s => new { s.StandardNumPiecesId, s.CutSizeId })
                    .IsUnique();

                modelBuilder.Entity<LiftResult>().HasNoKey();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error configuring database model: {ex.Message}\n\nApplication may need to be restarted.",
                    "Database Model Configuration Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
