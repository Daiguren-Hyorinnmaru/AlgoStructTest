using Microsoft.EntityFrameworkCore;
using DataBaseModels.Models;
using ServerAPI.DataBase.Repository;
using System.Data.SqlClient;

namespace ServerAPI.DataBase.UnitOfWork
{
    public class PC_ConfigWork
    {
        private DataContext _context;
        private Repository<RAM_Config> RAM_Config;
        private Repository<CPU_Config> CPU_Config;
        private Repository<PC_Config> PC_Config;

        public PC_ConfigWork(DataContext context)
        {
            _context = context;
            RAM_Config = new Repository<RAM_Config>(context);
            CPU_Config = new Repository<CPU_Config>(context);
            PC_Config = new Repository<PC_Config>(context);
        }

        public async Task<int> GetIdPC_ConfigAsync(RAM_Config ramConfig, CPU_Config cpuConfig)
        {
            try
            {
                PC_Config newConfig = new PC_Config()
                {
                    CPU_Id = await GetOrAddCPUConfigAsync(cpuConfig),
                    RAM_Id = await GetOrAddRAMConfigAsync(ramConfig)
                };

                var existingConfig = await PC_Config.FindAsync(pc =>
                    pc.CPU_Id == newConfig.CPU_Id &&
                    pc.RAM_Id == newConfig.RAM_Id);

                if (existingConfig.Any())
                {
                    return existingConfig.First().Id; // If configuration is found, return its Id
                }

                await PC_Config.AddAsync(newConfig);

                await _context.SaveChangesAsync();

                return newConfig.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled Error: " + ex.Message);
                throw;
            }
        }

        // Returns Id of CPU_Config, if not found adds a new one
        private async Task<int> GetOrAddCPUConfigAsync(CPU_Config cpuConfig)
        {
            // Check if the CPU configuration exists
            var existingCpu = await CPU_Config.FindAsync(cpu =>
                cpu.Name == cpuConfig.Name &&
                cpu.NumberOfCores == cpuConfig.NumberOfCores &&
                cpu.ThreadCount == cpuConfig.ThreadCount &&
                cpu.MaxClockSpeed == cpuConfig.MaxClockSpeed);

            if (existingCpu.Any())
            {
                return existingCpu.First().Id; // If exists, return its Id
            }

            // If not found, add a new configuration
            await CPU_Config.AddAsync(cpuConfig);
            await _context.SaveChangesAsync();

            return cpuConfig.Id;
        }

        // Returns Id of RAM_Config, if not found adds a new one
        private async Task<int> GetOrAddRAMConfigAsync(RAM_Config ramConfig)
        {
            // Check if the RAM configuration exists
            var existingRam = await RAM_Config.FindAsync(ram =>
                ram.Name == ramConfig.Name &&
                ram.TotalVisibleMemorySize == ramConfig.TotalVisibleMemorySize &&
                ram.Speed == ramConfig.Speed);

            if (existingRam.Any())
            {
                return existingRam.First().Id; // If exists, return its Id
            }

            // If not found, add a new configuration
            await RAM_Config.AddAsync(ramConfig);
            await _context.SaveChangesAsync();

            return ramConfig.Id;
        }
    }
}