using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DataBaseModels.Models;
using System.Data.SqlClient;

namespace ServerAPI.DataBase
{
    public partial class DataContext : DbContext
    {
        public DbSet<CPU_Config> CPU_Config { get; set; } = null!;
        public DbSet<DataType> DataType { get; set; } = null!;
        public DbSet<PC_Config> PC_Config { get; set; } = null!;
        public DbSet<RAM_Config> RAM_Config { get; set; } = null!;
        public DbSet<SortCollectionType> SortCollectionType { get; set; } = null!;
        public DbSet<SortConfig> SortConfig { get; set; } = null!;
        public DbSet<SortResult> SortResult { get; set; } = null!;
        public DbSet<SortsAlgorithm> SortsAlgorithm { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Console.WriteLine("EnsureDeleted");
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
