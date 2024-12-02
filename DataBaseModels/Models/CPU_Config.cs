using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Models
{
    public class CPU_Config
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfCores { get; set; }
        public int ThreadCount { get; set; }
        public int MaxClockSpeed { get; set; }

        public ICollection<PC_Config> PC_Configs { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not CPU_Config other) return false;
            return Name == other.Name &&
                NumberOfCores == other.NumberOfCores &&
                ThreadCount == other.ThreadCount &&
                MaxClockSpeed == other.MaxClockSpeed;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Name, NumberOfCores, ThreadCount, MaxClockSpeed);
    }
}
