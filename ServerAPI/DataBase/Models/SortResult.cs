namespace ServerAPI.DataBase.Models
{
    public class SortResult
    {
        public int Id { get; set; }
        public int SortConfigId {  get; set; }
        public int PC_ConfigId {  get; set; }
        public int Speed {  get; set; }

        public SortConfig? SortConfig { get; set; }
        public PC_Config? PC_Config { get; set; }
    }
}
