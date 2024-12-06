using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataBaseModels.Models
{
    public class SortResult
    {
        public int Id { get; set; }
        public  int Time { get; set; }
        public  int Length {  get; set; }
        public  string AlgorithmType {  get; set; }
        public  string CollectionType {  get; set; }
        public  string DataType {  get; set; }


        public int IDpcConfig {  get; set; }
        public PC_Config? PC_Config { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not SortResult other) return false;
            return Time == other.Time &&
                Length == other.Length &&
                AlgorithmType == other.AlgorithmType &&
                CollectionType == other.CollectionType &&
                DataType == other.DataType &&
                IDpcConfig == other.IDpcConfig;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Time, Length, AlgorithmType, CollectionType, DataType);
    }
}
