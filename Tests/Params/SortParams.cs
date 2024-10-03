using System.Collections.Generic;
using Tests.Algorithms;
using Tests.Factory;

namespace Tests.Params
{
    public class SortParams
    {
        public List<CollectionType> Collections { get; set; }
        public List<DataType> DataTypes { get; set; }
        public List<SortsAlgorithms> Algorithms { get; set; }
        public int LengthStart { get; set; }
        public int LengthEnd { get; set; }
        public int Step { get; set; }
    }
}
