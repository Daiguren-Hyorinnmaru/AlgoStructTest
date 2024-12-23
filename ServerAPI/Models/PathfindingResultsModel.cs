namespace ServerAPI.Models
{
    public class PathfindingResultsModel
    {
        public List<string> AlgorithmType { get; set; } = new List<string>();
        public List<string> PathfindingNameCPU { get; set; } = new List<string>();
    }
}
