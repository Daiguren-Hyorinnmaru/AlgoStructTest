using Microsoft.EntityFrameworkCore;
using DataBaseModels.Models;
using System.Reflection.Emit;

namespace ServerAPI.DataBase
{
    public partial class DataContext
    {
        private void ConfigSet(ModelBuilder modelBuilder)
        {
            ConfigureSortsAlgorithm(modelBuilder);
            ConfigureDataType(modelBuilder);
            ConfigureSortCollectionType(modelBuilder);
            ConfigureSortConfig(modelBuilder);
            ConfigureRAM_Config(modelBuilder);
            ConfigureCPU_Config(modelBuilder);
            ConfigurePC_Config(modelBuilder);
            ConfigureSortResult(modelBuilder);
        }

        private void ConfigureSortsAlgorithm(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<SortsAlgorithm>();

            entity.HasKey(sa => sa.Id);
            entity.Property(sa => sa.NameAlgorithm)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(sa => sa.NameAlgorithm)
                  .IsUnique();
        }

        private void ConfigureDataType(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<DataType>();

            entity.HasKey(dt => dt.Id);
            entity.Property(dt => dt.NameDataType)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(dt => dt.NameDataType)
                  .IsUnique();
        }

        private void ConfigureSortCollectionType(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<SortCollectionType>();

            entity.HasKey(sct => sct.Id);
            entity.Property(sct => sct.NameCollection)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(sct => sct.NameCollection)
                  .IsUnique();
        }

        private void ConfigureSortConfig(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<SortConfig>();

            entity.HasKey(sc => sc.Id);

            entity.Property(sct => sct.SortsAlgorithmId)
                  .IsRequired();

            entity.Property(sct => sct.SortsCollectionId)
                  .IsRequired();

            entity.Property(sct => sct.DataTypeId)
                  .IsRequired();

            entity.Property(sct => sct.Length)
                  .IsRequired();

            entity.HasOne(sc => sc.SortsAlgorithm)
                  .WithMany(sa => sa.SortConfigs) 
                  .HasForeignKey(sc => sc.SortsAlgorithmId)
                  .OnDelete(DeleteBehavior.Cascade); 

            entity.HasOne(sc => sc.SortsCollectionType)
                  .WithMany(sct => sct.SortConfigs)
                  .HasForeignKey(sc => sc.SortsCollectionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sc => sc.DataType)
                  .WithMany(dt => dt.SortConfigs) 
                  .HasForeignKey(sc => sc.DataTypeId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(sc => new { sc.SortsAlgorithmId, sc.SortsCollectionId, sc.DataTypeId })
                  .IsUnique(); 
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

            entity.HasIndex(sc => new { sc.CPU_Id, sc.RAM_Id})
                  .IsUnique();
        }

        private void ConfigureSortResult(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<SortResult>();

            entity.HasKey(sc => sc.Id);

            entity.Property(sct => sct.SortConfigId)
                  .IsRequired();

            entity.Property(sct => sct.PC_ConfigId)
                  .IsRequired();

            entity.Property(sct => sct.Speed)
                  .IsRequired();

            entity.HasOne(sc => sc.SortConfig)
                  .WithMany(sct => sct.SortResults)
                  .HasForeignKey(sc => sc.SortConfigId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sc => sc.PC_Config)
                  .WithMany(sct => sct.SortResults) 
                  .HasForeignKey(sc => sc.PC_ConfigId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}