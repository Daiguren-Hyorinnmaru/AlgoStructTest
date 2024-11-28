namespace ServerAPI.DataBase.Models
{
    public class SortCollectionType
    {
        public int Id { get; set; }

        public string NameCollection { get; set; }
        public ICollection<SortConfig> SortConfigs { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not SortCollectionType other) return false;
            return NameCollection == other.NameCollection;
        }

        public override int GetHashCode() => NameCollection.GetHashCode();
    }
}
