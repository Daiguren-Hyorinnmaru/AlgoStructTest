using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Algorithms;

namespace Tests.Factory
{
    public enum DataType
    {
        Integer,
        String,
        Double
    }

    public class DataFactory
    {
        public object RandomValueType(DataType dataType)
        {
            Random random = new Random();

            switch (dataType)
            {
                case DataType.Integer:
                    return random.Next();
                case DataType.String:
                    int length = random.Next(5, 15);
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    return new string(Enumerable.Repeat(chars, length)
                                                .Select(s => s[random.Next(s.Length)]).ToArray());
                case DataType.Double:
                    return random.NextDouble() * random.Next(1, 100);
            }

            return null;
        }

    }
}
