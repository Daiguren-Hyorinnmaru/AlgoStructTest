namespace ServerAPI.Models
{
    public class IndexModel
    {
        public SortResultModel Sort { get; set; } = new SortResultModel();
        public PathfindingResultsModel Pathfinding { get; set; } = new PathfindingResultsModel();
    }
}
