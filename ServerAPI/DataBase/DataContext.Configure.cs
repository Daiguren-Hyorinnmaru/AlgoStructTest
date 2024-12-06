using Microsoft.EntityFrameworkCore;
using DataBaseModels.Models;
using System.Reflection.Emit;

namespace ServerAPI.DataBase
{
    public partial class DataContext
    {
        private void ConfigSet(ModelBuilder modelBuilder)
        {
            ConfigureRAM_Config(modelBuilder);
            ConfigureCPU_Config(modelBuilder);
            ConfigurePC_Config(modelBuilder);
            ConfigureSortResult(modelBuilder);
        }


        private void ConfigureRAM_Config(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<RAM_Config>();

            entity.HasKey(sa => sa.Id);

            entity.Property(sa => sa.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(sa => sa.TotalVisibleMemorySize)
                  .IsRequired();

            entity.Property(sa => sa.Speed)
                  .IsRequired();

            entity.HasIndex(sc => new { sc.Name, sc.TotalVisibleMemorySize, sc.Speed })
                  .IsUnique();
        }

        private void ConfigureCPU_Config(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<CPU_Config>();

            entity.HasKey(sa => sa.Id);

            entity.Property(sa => sa.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(sa => sa.NumberOfCores)
                  .IsRequired();

            entity.Property(sa => sa.ThreadCount)
                  .IsRequired();

            entity.Property(sa => sa.MaxClockSpeed)
                  .IsRequired();

            entity.HasIndex(sc => new { sc.Name, sc.NumberOfCores, sc.ThreadCount, sc.MaxClockSpeed })
                  .IsUnique();
        }

        private void ConfigurePC_Config(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<PC_Config>();

            entity.HasKey(sc => sc.Id);

            entity.Property(sct => sct.CPU_Id)
                  .IsRequired();

            entity.Property(sct => sct.RAM_Id)
                  .IsRequired();

            entity.HasOne(sc => sc.CPU_Config)
                  .WithMany(sct => sct.PC_Configs)
                  .HasForeignKey(sc => sc.CPU_Id)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sc => sc.RAM_Config)
                  .WithMany(sct => sct.PC_Configs)
                  .HasForeignKey(sc => sc.RAM_Id)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(sc => new { sc.CPU_Id, sc.RAM_Id })
                  .IsUnique();
        }

        private void ConfigureSortResult(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<SortResult>();

            entity.HasKey(sc => sc.Id);

            entity.Property(sct => sct.Time).IsRequired();
            entity.Property(sct => sct.Length).IsRequired();
            entity.Property(sct => sct.AlgorithmType).IsRequired();
            entity.Property(sct => sct.CollectionType).IsRequired();
            entity.Property(sct => sct.DataType).IsRequired();

            entity.HasOne(sc => sc.PC_Config)
                  .WithMany(sct => sct.SortResults)
                  .HasForeignKey(sc => sc.IDpcConfig)
                  .OnDelete(DeleteBehavior.Cascade);
        }
        private void ConfigurePathfindingResults(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<PathfindingResults>();

            entity.HasKey(sc => sc.Id);

            entity.Property(sct => sct.Time).IsRequired();
            entity.Property(sct => sct.LengthPath).IsRequired();
            entity.Property(sct => sct.Algotithm).IsRequired();

            entity.HasOne(sc => sc.PC_Config)
                  .WithMany(sct => sct.PathfindingResults)
                  .HasForeignKey(sc => sc.IDpcConfig)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}