using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Models
{
    public class RAM_Config
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalVisibleMemorySize { get; set; }
        public int Speed { get; set; }

        public ICollection<PC_Config> PC_Configs { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not RAM_Config other) return false;
            return Name == other.Name &&
                TotalVisibleMemorySize == other.TotalVisibleMemorySize &&
                Speed == other.Speed;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Name, TotalVisibleMemorySize, Speed);
    }
}
