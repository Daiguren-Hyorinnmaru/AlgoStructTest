using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Result
{
    public class SendResult
    {
        public string RamName { get; set; }
        public int TotalVisibleMemorySize { get; set; }
        public int Speed { get; set; }
        public string CpuName { get; set; }
        public int NumberOfCores { get; set; }
        public int ThreadCount { get; set; }
        public int MaxClockSpeed { get; set; }

        public List<SortResult> SortsResults { get; set; }
        public List<PathfindingResult> PathfindingResults { get; set; }
    }
}
