using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Testing;

namespace Tests.Params
{
    public class PathfindingParams
    {
        public List<PathfindingAlgotithm> Algotithm { get; set; } = new List<PathfindingAlgotithm>();
        public int CountIteration {  get; set; }
    }
}
