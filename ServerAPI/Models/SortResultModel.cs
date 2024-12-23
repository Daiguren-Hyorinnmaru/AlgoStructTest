namespace ServerAPI.Models
{
    public class SortResultModel
    {
        public List<string> AlgorithmType { get; set; } = new List<string>();
        public List<string> CollectionType { get; set; } = new List<string>();
        public List<string> DataType { get; set; } = new List<string>();
        public List<string> NameCPU { get; set; } = new List<string>();
    }
}
