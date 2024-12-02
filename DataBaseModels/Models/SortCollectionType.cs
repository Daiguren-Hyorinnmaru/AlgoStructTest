using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Models
{
    public class SortCollectionType
    {
        public int Id { get; set; }

        public string NameCollection { get; set; }
        public ICollection<SortConfig> SortConfigs { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not SortCollectionType other) return false;
            return string.Equals(this.NameCollection, other.NameCollection, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode() =>
            NameCollection.ToLowerInvariant().GetHashCode();
    }
}
