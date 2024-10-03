using Tests.Algorithms;
using Tests.Factory;

namespace Tests.Result
{
    public class SortResult : IResult
    {
        public CollectionType Collection { get; set; }
        public DataType DataType { get; set; }
        public SortsAlgorithms Algorithm { get; set; }
        public int Length { get; set; }
        public long Time { get; set; }
    }
}
