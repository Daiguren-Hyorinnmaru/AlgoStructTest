using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Models
{
    public class PathfindingResults
    {
        public int Id { get; set; }
        public double LengthPath { get; set; }
        public int Time { get; set; }
        public string Algotithm { get; set; }


        public int IDpcConfig { get; set; }
        public PC_Config? PC_Config { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not PathfindingResults other) return false;
            return Time == other.Time &&
                LengthPath == other.LengthPath &&
                Algotithm == other.Algotithm;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Time, LengthPath, Algotithm);
    }
}
