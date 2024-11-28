using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerAPI.DataBase.Models
{
    public class SortsAlgorithm
    {
        public int Id { get; set; }

        public string NameAlgorithm { get; set; }
        public ICollection<SortConfig> SortConfigs { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not SortsAlgorithm other) return false;
            return NameAlgorithm == other.NameAlgorithm;
        }

        public override int GetHashCode() => NameAlgorithm.GetHashCode();
    }
}
