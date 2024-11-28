namespace ServerAPI.DataBase.Models
{
    public class DataType
    {
        public int Id { get; set; }

        public string NameDataType { get; set; }
        public ICollection<SortConfig> SortConfigs { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not DataType other) return false;
            return NameDataType == other.NameDataType;
        }

        public override int GetHashCode() => NameDataType.GetHashCode();
    }
}
