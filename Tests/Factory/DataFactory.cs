using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Params;

namespace Tests.Factory
{
    public enum DataType
    {
        Integer,
        String,
        Double
    }

    internal class DataFactory
    {
        private Random random;
        public DataParams DataParams { private get; set; }
        private Dictionary<DataType, Func<object>> dataTypeGenerators;

        public DataFactory(DataParams dataParams = null)
        {
            DataParams = dataParams ?? new DataParams();
            random = new Random();
            InitializeDataTypeGenerators();
        }

        private void InitializeDataTypeGenerators()
        {
            dataTypeGenerators = new Dictionary<DataType, Func<object>>
            {
                { DataType.Integer, GenerateRandomInteger },
                { DataType.String, GenerateRandomString },
                { DataType.Double, GenerateRandomDouble }
            };
        }

        public object RandomValueType(DataType dataType) =>
            dataTypeGenerators[dataType]();

        private object GenerateRandomInteger() =>
            random.Next(DataParams.IntMin, DataParams.IntMax);

        private object GenerateRandomString()
        {
            int length = random.Next(DataParams.StringMinLength, DataParams.StringMaxLength);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private object GenerateRandomDouble() =>
            random.NextDouble() * (DataParams.DoubleMax - DataParams.DoubleMin) + DataParams.DoubleMin;
    }
}
