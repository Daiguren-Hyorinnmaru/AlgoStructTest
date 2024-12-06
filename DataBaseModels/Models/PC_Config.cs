using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Models
{
    public class PC_Config
    {
        public int Id { get; set; }
        public int CPU_Id { get; set; }
        public int RAM_Id { get; set; }

        public CPU_Config? CPU_Config { get; set; }
        public RAM_Config? RAM_Config { get; set; }
        public ICollection<SortResult> SortResults { get; set; }
        public ICollection<PathfindingResults> PathfindingResults { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (PC_Config)obj;
            return CPU_Id == other.CPU_Id &&
                   RAM_Id == other.RAM_Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CPU_Id, RAM_Id);
        }
    }
}
