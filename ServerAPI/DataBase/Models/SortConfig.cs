using System.ComponentModel.DataAnnotations;

namespace ServerAPI.DataBase.Models
{
    public class SortConfig
    {
        public int Id { get; set; }
        public int SortsAlgorithmId { get; set; }
        public int SortsCollectionId { get; set; }
        public int DataTypeId { get; set; }

        public SortsAlgorithm? SortsAlgorithm { get; set; }
        public SortCollectionType? SortsCollectionType { get; set; }
        public DataType? DataType { get; set; }
        public ICollection<SortResult> SortResults { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (SortConfig)obj;
            return SortsAlgorithmId == other.SortsAlgorithmId &&
                   SortsCollectionId == other.SortsCollectionId &&
                   DataTypeId == other.DataTypeId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SortsAlgorithmId, SortsCollectionId, DataTypeId);
        }
    }
}
