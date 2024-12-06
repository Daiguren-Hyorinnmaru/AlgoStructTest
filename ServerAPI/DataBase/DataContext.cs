using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DataBaseModels.Models;
using System.Data.SqlClient;

namespace ServerAPI.DataBase
{
    public partial class DataContext : DbContext
    {
        public DbSet<CPU_Config> CPU_Config { get; set; } = null!;
        public DbSet<PC_Config> PC_Config { get; set; } = null!;
        public DbSet<RAM_Config> RAM_Config { get; set; } = null!;
        public DbSet<SortResult> SortResult { get; set; } = null!;
        public DbSet<PathfindingResults> PathfindingResults { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigSet(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
